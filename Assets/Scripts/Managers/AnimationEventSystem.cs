using System;
using UnityEngine;

public class AnimationEventSystem : MonoBehaviour
{
    private static Action _animationFinishedEvent;

    public static Action AnimationFinishedEvent
        { get { return _animationFinishedEvent; } set { _animationFinishedEvent = value; } }

    // called in an animation
    public void FinishAnimation()
    {
        AnimationFinishedEvent?.Invoke();
    }
}
