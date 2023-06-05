using UnityEngine;

[CreateAssetMenu(fileName = "New SoundSettingsSO", menuName = "Game Settings/Sound Settings")]
public class SoundSettingsSO : ScriptableObject
{
    [SerializeField]
    private bool _muteMusic;
    [SerializeField]
    private bool _muteSoundEffects;
    [SerializeField]
    [Range(0, 1)] private float _musicVolume;
    [SerializeField]
    [Range(0, 1)] private float _soundEffectsVolume;

    public bool MuteMusic { get { return _muteMusic; } set { _muteMusic = value; } }
    public bool MuteSoundEffects { get { return _muteSoundEffects; } set { _muteSoundEffects = value; } }
    public float MusicVolume { get { return _musicVolume; } set { _musicVolume = value; } }
    public float SoundEffectsVolume { get { return _soundEffectsVolume; } set { _soundEffectsVolume = value; } }
}
