using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [SerializeField]
    private SoundSettingsSO _soundSettings;

    [SerializeField]
    private bool _manageMusic, _manageSoundEffects;

    [SerializeField]
    private Toggle _muteToggle;

    [SerializeField]
    private Slider _volumeSlider;

    void Awake()
    {
        if (_manageMusic)
        {
            _volumeSlider.value = _soundSettings.MusicVolume;
            _muteToggle.isOn = _soundSettings.MuteMusic;
        }
        else if (_manageSoundEffects)
        {
            _volumeSlider.value = _soundSettings.SoundEffectsVolume;
            _muteToggle.isOn = _soundSettings.MuteSoundEffects;
        }

        _muteToggle.onValueChanged.RemoveAllListeners();
        _muteToggle.onValueChanged.AddListener(isOn => MuteAudio(isOn));

        _volumeSlider.onValueChanged.RemoveAllListeners();
        _volumeSlider.onValueChanged.AddListener(volume => ChangeVolume(volume));
    }

    private void MuteAudio(bool isOn)
    {
        _volumeSlider.interactable = !isOn;

        if (_manageMusic)
        {
            SoundManager.Instance.ToggleMusic(isOn);
            _soundSettings.MuteMusic = isOn;
        }
        else if (_manageSoundEffects)
        {
            SoundManager.Instance.ToggleSoundEffects(isOn);
            _soundSettings.MuteSoundEffects = isOn;
        }
    }

    private void ChangeVolume(float volume)
    {
        if (_manageMusic)
        {
            SoundManager.Instance.ChangeMusicVolume(volume);
            _soundSettings.MusicVolume = volume;
        }
        else if (_manageSoundEffects)
        {
            SoundManager.Instance.ChangeEffectsVolume(volume);
            _soundSettings.SoundEffectsVolume = volume;
        }
    }
}
