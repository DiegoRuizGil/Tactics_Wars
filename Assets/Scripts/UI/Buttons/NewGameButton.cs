using System.IO;
using UnityEngine;
using TMPro;

public class NewGameButton : MonoBehaviour
{
    [SerializeField]
    private TextAsset _defaultSave;

    [SerializeField]
    private SaveToLoadSO _saveToLoadSO;

    [SerializeField]
    private TextMeshProUGUI _errorText;

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

        LevelManager.Instance.LoadScene("GameScene");
    }
}
