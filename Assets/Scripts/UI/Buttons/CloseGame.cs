using UnityEngine;

public class CloseGame : MonoBehaviour
{
    public void QuitGame()
    {
        Debug.Log("Cerramos la aplicación");
        Application.Quit();
    }
}
