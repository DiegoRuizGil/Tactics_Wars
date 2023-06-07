using UnityEngine;

public class SceneMusicSetter : MonoBehaviour
{
    [SerializeField]
    private AudioClip _sceneMusicClip;
    
    void Start()
    {
        SoundManager.Instance.SetMusic(_sceneMusicClip);
    }
}
