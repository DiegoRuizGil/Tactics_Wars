using UnityEngine;

public class CloseGame : MonoBehaviour
{
    public void QuitGame()
    {
        Debug.Log("Cerramos la aplicaci�n");
        Application.Quit();
    }
}
