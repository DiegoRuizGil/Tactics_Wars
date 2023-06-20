using UnityEngine;

public class AnimationsAudioClip : MonoBehaviour
{
    [SerializeField]
    private AudioClip _attackClip;
    [SerializeField]
    private AudioClip _moveClip;
    [SerializeField]
    private AudioClip _hurtClip;
    [SerializeField]
    private AudioClip _deathClip;

    public void PlayAttackClip()
    {
        if (_attackClip != null)
            SoundManager.Instance.PlaySoundEffect(_attackClip);
    }

    public void PlayMoveClip()
    {
        if (_moveClip != null)
            SoundManager.Instance.PlaySoundEffectLoop(_moveClip);
    }

    public void PlayHurtClip()
    {
        if (_hurtClip != null)
            SoundManager.Instance.PlaySoundEffect(_hurtClip);
    }

    public void PlayDeathClip()
    {
        if (_deathClip != null)
            SoundManager.Instance.PlaySoundEffect(_deathClip);
    }
}
