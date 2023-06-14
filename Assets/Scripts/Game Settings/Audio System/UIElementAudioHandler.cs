using UnityEngine;
using UnityEngine.EventSystems;

public class UIElementAudioHandler : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerUpHandler
{
    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip _onPointerClickClip;
    [SerializeField]
    private AudioClip _onPointerEnterClip;
    [SerializeField]
    private AudioClip _onPointerUpClip;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_onPointerClickClip != null)
            SoundManager.Instance.PlaySoundEffect(_onPointerClickClip);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_onPointerEnterClip != null)
            SoundManager.Instance.PlaySoundEffect(_onPointerEnterClip);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_onPointerUpClip != null)
            SoundManager.Instance.PlaySoundEffect(_onPointerUpClip);
    }
}
