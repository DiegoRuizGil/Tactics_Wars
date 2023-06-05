using UnityEngine;

public sealed class SoundManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource _musicSource, _effectSource;

    [SerializeField]
    private SoundSettingsSO _soundSettings;

    private static SoundManager _instance;

    public static SoundManager Instance { get { return _instance; } }

    private SoundManager() { }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // load sound settings
        ChangeEffectsVolume(_soundSettings.SoundEffectsVolume);
        ChangeMusicVolume(_soundSettings.MusicVolume);

        ToggleSoundEffects(_soundSettings.MuteSoundEffects);
        ToggleMusic(_soundSettings.MuteMusic);
    }

    public void PlaySoundEffect(AudioClip clip)
    {
        _effectSource.PlayOneShot(clip);
    }

    public void ChangeEffectsVolume(float value)
    {
        _effectSource.volume = Mathf.Pow(value, 2f);
    }

    public void ChangeMusicVolume(float value)
    {
        _musicSource.volume = Mathf.Pow(value, 2f);
    }

    public void ToggleSoundEffects(bool mute)
    {
        _effectSource.mute = mute;
    }

    public void ToggleMusic(bool mute)
    {
        _musicSource.mute = mute;
    }
}
