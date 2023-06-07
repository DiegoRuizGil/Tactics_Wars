using System;
using UnityEngine;

public class SceneInitializer : MonoBehaviour
{
    [SerializeField]
    private TextAsset _textJSON;

    [SerializeField]
    private EntitiesPrefabsSO _entitiesPrefabs;

    [SerializeField]
    private SaveToLoadSO _saveToLoad;

    [ContextMenu("Read JSON")]
    public void ReadJSON()
    {
        
    }

    public void InitializeSceneData() // called from listener
    {
        // SceneData sceneData = JsonUtility.FromJson<SceneData>(_textJSON.text);

        SceneData sceneData = SaveSystem.Load(_saveToLoad.SaveToLoad.Name, _saveToLoad.IsNewGame);

        InstantiateEntities(sceneData.entitiesData);

        SetResources(sceneData.resources);

        //GameManager.Instance.Turn = sceneData.gameData.turn;
        //GameManager.Instance.CurrentTeam = sceneData.gameData.currentTeam;

        GameManager.Instance.UpdateTopHUD();
    }

    private void InstantiateEntities(EntityData[] entitiesData)
    {
        foreach (EntityData entity in entitiesData)
        {
            if (Grid.Instance.CheckPositionOutOfBounds(entity.position))
                continue;
            if (!Enum.IsDefined(typeof(TeamEnum), entity.team))
                continue;

            if (_entitiesPrefabs.TryGetPrefab(entity.name, out Entity prefab))
            {
                Entity instance = GameManager.Instance.InstantiateEntity(
                        prefab,
                        entity.position,
                        entity.team
                    );

                if (entity.currentHealth > 0)
                    instance.CurrentHealth = entity.currentHealth;

                if (instance is Unit)
                {
                    Unit unit = instance as Unit;

                    unit.HasMoved = entity.hasMoved;
                    unit.HasFinished = entity.hasFinished;
                    unit.JustInstantiated = entity.justInstantiated;
                }
            }
            else
            {
                Debug.LogWarning($"Nombre de entidad no encontrado: {entity.name}");
            }
        }
    }

    private void SetResources(ResourceData[] resources)
    {
        foreach (ResourceData data in resources)
        {
            if (!Enum.IsDefined(typeof(TeamEnum), data.team))
                continue;

            GameManager.Instance.SetFoodResources(
                    data.team,
                    data.food
                );

            GameManager.Instance.SetGoldResources(
                    data.team,
                    data.gold
                );
        }
    }
}
