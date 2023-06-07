using System.IO;
using UnityEngine;

public class ExitCurrentGameButton : MonoBehaviour
{
    [SerializeField]
    private SaveToLoadSO _saveToLoadSO;

    public void ExitGame(bool save)
    {
        if (save)
        {
            if (_saveToLoadSO.IsNewGame)
            {
                FileInfo newFile = SaveSystem.Save();
                _saveToLoadSO.SaveToLoad = newFile;
                _saveToLoadSO.IsNewGame = false;
            }
            else
            {
                SaveSystem.Save(_saveToLoadSO.SaveToLoad.Name);
            }
        }

        LevelManager.Instance.LoadScene("MainMenu");
    }
}
