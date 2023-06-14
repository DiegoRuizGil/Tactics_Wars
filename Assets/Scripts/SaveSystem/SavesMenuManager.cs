using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SavesMenuManager : MonoBehaviour
{
    [SerializeField]
    private UnityEvent _loadErrorEvent;

    [SerializeField]
    private Button _loadSaveButtonPrefab;

    [SerializeField]
    private SaveToLoadSO _saveToLoadSO;

    [SerializeField]
    private EntitiesPrefabsSO _entitiesPrefabs;

    private void Awake()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(SaveSystem.SAVE_FOLDER);
        List<FileInfo> saveFiles = directoryInfo.GetFiles("*.json").ToList();
        saveFiles.Sort((a, b) => b.LastWriteTime.CompareTo(a.LastWriteTime));

        foreach (FileInfo fileInfo in saveFiles)
        {
            Button button = Instantiate(_loadSaveButtonPrefab, this.transform);

            try
            {
                SceneData sceneData = SaveSystem.Load(fileInfo.Name, false);
                if (SaveSystem.IsValidData(sceneData))
                {
                    LoadGameButtonController lgbc = button.gameObject.GetComponent<LoadGameButtonController>();
                    lgbc.SaveDateText = fileInfo.LastWriteTime.ToString();
                    lgbc.FoodAmountText = sceneData.resources[0].food.ToString();
                    lgbc.GoldAmountText = sceneData.resources[0].gold.ToString();
                    lgbc.currentTurnText = Mathf.CeilToInt(sceneData.gameData.turn / 2f).ToString();

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
                }
            }
            catch (System.ArgumentException ex)
            {
                Debug.LogWarning($"[{ex.GetType()}] {ex.Message}");
            }

            button.onClick.AddListener(() => OnButtonClick(fileInfo));
        }
    }

    private void OnButtonClick(FileInfo fileInfo)
    {
        try
        {
            if (!SaveSystem.IsValidData(SaveSystem.Load(fileInfo.Name, false)))
            {
                if (_loadErrorEvent != null)
                    _loadErrorEvent.Invoke();
            }
            else
            {
                _saveToLoadSO.SaveToLoad = fileInfo;
                _saveToLoadSO.IsNewGame = false;

                LevelManager.Instance.LoadScene("GameScene");
            }
        }
        catch (System.ArgumentException ex)
        {
            Debug.LogWarning($"[{ex.GetType()}] {ex.Message}");

            if (_loadErrorEvent != null)
                _loadErrorEvent.Invoke();
        }
    }
    
}
