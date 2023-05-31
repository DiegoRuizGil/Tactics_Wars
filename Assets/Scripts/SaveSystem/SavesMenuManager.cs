using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SavesMenuManager : MonoBehaviour
{
    [SerializeField]
    private Button _loadSaveButtonPrefab;

    [SerializeField]
    private SaveToLoadSO _saveToLoadSO;

    [SerializeField]
    private EntitiesPrefabsSO _entitiesPrefabs;

    private void Awake()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(SaveSystem.SAVE_FOLDER);
        FileInfo[] saveFiles = directoryInfo.GetFiles("*.json");

        foreach (FileInfo fileInfo in saveFiles)
        {
            Button button = Instantiate(_loadSaveButtonPrefab, this.transform);

            SceneData sceneData = SaveSystem.Load(fileInfo.Name, false);
            LoadGameButtonController lgbc = button.gameObject.GetComponent<LoadGameButtonController>();
            lgbc.SaveDateText = fileInfo.LastWriteTime.ToString();
            lgbc.FoodAmountText = sceneData.resources[0].food.ToString();
            lgbc.GoldAmountText = sceneData.resources[0].gold.ToString();
            lgbc.currentTurnText = sceneData.gameData.turn.ToString();

            int entities = 0;
            foreach (EntityData entity in sceneData.entitiesData)
            {
                if (entity.team == TeamEnum.BLUE && _entitiesPrefabs.TryGetPrefab(entity.name, out Entity prefab))
                {
                    if (prefab is Unit)
                        entities++;
                } 
            }
            lgbc.EntitiesAmountText = entities.ToString();

            button.onClick.AddListener(() => OnButtonClick(fileInfo));
        }
    }

    private void OnButtonClick(FileInfo fileInfo)
    {
        _saveToLoadSO.SaveToLoad = fileInfo;
        _saveToLoadSO.IsNewGame = false;
        
        LevelManager.Instance.LoadScene("GameScene");
    }
    
}
