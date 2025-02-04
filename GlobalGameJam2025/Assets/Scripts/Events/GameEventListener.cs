using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    public GameEvent gameEvent;
    public UnityEvent callback;

    private void OnEnable()
    {
        gameEvent.Register(this);
    }
    private void OnDisable()
    {
        gameEvent.Unregister(this);
    }

    public void OnEventRaised()
    {
        callback?.Invoke();
    }
}
