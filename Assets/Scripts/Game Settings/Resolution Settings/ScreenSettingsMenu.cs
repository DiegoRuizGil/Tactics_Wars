using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScreenSettingsMenu : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown _resolutionDropdown;
    [SerializeField]
    private Toggle _fullScreenToggle;

    [SerializeField]
    private ScreenSettingsSO _screenSettingsSO;

    private Resolution[] _resolutions;

    void Start()
    {
        _fullScreenToggle.isOn = _screenSettingsSO.FullScreen;

        _resolutions = Screen.resolutions;

        List<string> options = new List<string>();
        int currentResolution = 0;
        for (int i=0; i < _resolutions.Length; i++)
        {
            //if ((_resolutions[i].width * 9) != (_resolutions[i].height * 16))
            //    continue;

            options.Add($"{_resolutions[i].width}x{_resolutions[i].height} {_resolutions[i].refreshRate}Hz");

            if (_resolutions[i].Equals(Screen.currentResolution))
            {
                currentResolution = i;
            }
        }

        _resolutionDropdown.ClearOptions();
        _resolutionDropdown.AddOptions(options);
        _resolutionDropdown.value = currentResolution;
        _resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int option)
    {
        Resolution resolution = _resolutions[option];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        _screenSettingsSO.CurrentResolution = resolution;
    }

    public void SetFullScreen(bool fullScreen)
    {
        Screen.fullScreen = fullScreen;
        _screenSettingsSO.FullScreen = fullScreen;
    }
}
