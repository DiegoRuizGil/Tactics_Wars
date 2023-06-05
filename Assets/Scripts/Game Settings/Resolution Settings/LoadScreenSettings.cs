using UnityEngine;

public class LoadScreenSettings : MonoBehaviour
{
    [SerializeField]
    private ScreenSettingsSO _screenSettingsSO;

    void Start()
    {
        if (_screenSettingsSO.FullScreen)
        {
            Screen.fullScreen = _screenSettingsSO.FullScreen;
        } 
        else
        {
            Screen.SetResolution(
                _screenSettingsSO.CurrentResolution.width,
                _screenSettingsSO.CurrentResolution.height,
                Screen.fullScreen
            );
        }
    }
}
