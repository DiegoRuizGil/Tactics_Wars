using UnityEngine;

public class FinishGameButton : MonoBehaviour
{
    [SerializeField]
    private SaveToLoadSO _saveToLoadSO;

    public void FinishGame()
    {
        if (!_saveToLoadSO.IsNewGame)
        {
            SaveSystem.DeleteSaveFile(_saveToLoadSO.SaveToLoad.Name);
        }

        LevelManager.Instance.LoadScene("MainMenu");
    }
}
