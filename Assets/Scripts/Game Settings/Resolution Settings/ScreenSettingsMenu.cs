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

    private List<Resolution> _resolutionsList;

    void Start()
    {
        _fullScreenToggle.isOn = _screenSettingsSO.FullScreen;

        _resolutionsList = new List<Resolution>();
        Resolution[] resolutions = Screen.resolutions;

        List<string> options = new List<string>();
        int currentResolution = 0;
        int index = 0;
        foreach (Resolution resolution in resolutions)
        {
            if ((resolution.width * 9) != (resolution.height * 16))
                continue;

            _resolutionsList.Add(resolution);
            options.Add($"{resolution.width}x{resolution.height} {resolution.refreshRate}Hz");

            if (resolution.Equals(Screen.currentResolution))
            {
                Debug.Log($"Seleccionamos nueva resolucion: {resolution.width}x{resolution.height}");
                currentResolution = index;
            }

            index++;
        }

        _resolutionDropdown.ClearOptions();
        _resolutionDropdown.AddOptions(options);
        _resolutionDropdown.value = currentResolution;
        _resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int option)
    {
        Resolution resolution = _resolutionsList[option];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        _screenSettingsSO.CurrentResolution = resolution;
    }

    public void SetFullScreen(bool fullScreen)
    {
        Screen.fullScreen = fullScreen;
        _screenSettingsSO.FullScreen = fullScreen;
    }
}
