using UnityEngine;
using UnityEngine.Events;

/*
 * T   -> type
 * E   -> Event
 * EUR -> UnityEventResponse
*/
public abstract class BaseGameEventListener<T, E, EUR> : MonoBehaviour, IGameEventListener<T> where E : BaseGameEvent<T> where EUR : UnityEvent<T>
{
    [SerializeField] private E _gameEvent;
    public E GameEvent { get { return _gameEvent; } set { _gameEvent = value; } }

    [SerializeField] private EUR _unityEventResponse;

    public void OnEnable()
    {
        if (_gameEvent == null)
            return;

        GameEvent.RegisterListener(this);
    }

    public void OnDisable()
    {
        if (_gameEvent == null)
            return;

        GameEvent.UnregisterListener(this);
    }

    public void OnEventRaise(T item)
    {
        if (_unityEventResponse != null)
        {
            _unityEventResponse.Invoke(item);
        }
    }
}
