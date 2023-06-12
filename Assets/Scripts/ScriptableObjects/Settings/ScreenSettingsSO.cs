using UnityEngine;

[CreateAssetMenu(fileName = "New ScreenSettingsSO", menuName = "Game Settings/Screen Settings")]
public class ScreenSettingsSO : ScriptableObject
{
    [SerializeField]
    private bool _fullScreen = true;
    [SerializeField]
    private Resolution _currentResolution;

    public bool FullScreen { get { return _fullScreen; } set { _fullScreen = value; } }
    public Resolution CurrentResolution
    {
        get { return _currentResolution; }
        set
        {
            Debug.Log($"Seteamos nueva resolucion: {value.width}x{value.height}");
            _currentResolution = value;
        }
    }
}
