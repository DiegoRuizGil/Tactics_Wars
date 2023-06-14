using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class NewGameButton : MonoBehaviour
{
    [SerializeField]
    private UnityEvent _loadErrorEvent;

    [SerializeField]
    private TextAsset _defaultSave;

    [SerializeField]
    private SaveToLoadSO _saveToLoadSO;

    public void LoadNewGame()
    {
        SaveSystem.Init();

        string savePath = SaveSystem.NEW_GAME_SAVE_FOLDER + _defaultSave.name + ".json";

        if (!File.Exists(savePath))
        {
            File.WriteAllText(savePath, _defaultSave.text);
        }
        
        _saveToLoadSO.SaveToLoad = new FileInfo(savePath);
        _saveToLoadSO.IsNewGame = true;

        try
        {
            if (!SaveSystem.IsValidData(SaveSystem.Load(_defaultSave.name + ".json", true)))
            {
                if (_loadErrorEvent != null)
                    _loadErrorEvent.Invoke();
            }
            else
            {
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
