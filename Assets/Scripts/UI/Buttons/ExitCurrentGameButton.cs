using UnityEngine;

public class ExitCurrentGameButton : MonoBehaviour
{
    public void ExitGame()
    {
        LevelManager.Instance.LoadScene("MainMenu");
    }
}
