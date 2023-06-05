using UnityEngine;

[CreateAssetMenu(fileName = "New ScreenSettingsSO", menuName = "Game Settings/Screen Settings")]
public class ScreenSettingsSO : ScriptableObject
{
    private bool _fullScreen = true;
    private Resolution _currentResolution;

    public bool FullScreen { get { return _fullScreen; } set { _fullScreen = value; } }
    public Resolution CurrentResolution { get { return _currentResolution; } set { _currentResolution = value; } }
}
