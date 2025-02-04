using System;

internal interface IEventBinding<T>
{
    public Action<T> OnEvent { get; set; }
    public Action OnEventNoArgs { get; set; }
}

public class EventBinding<T> : IEventBinding<T> where T : IEvent
{
    Action<T> onEvent = _ => { };
    Action onEventNoArgs = () => { };
    private Action<PlayerData> playerReconnected;

    Action<T> IEventBinding<T>.OnEvent
    {
        get => onEvent;
        set => onEvent = value;
    }

    Action IEventBinding<T>.OnEventNoArgs
    {
        get => onEventNoArgs;
        set => onEventNoArgs = value;
    }

    public EventBinding(Action<T> onEvent) => this.onEvent = onEvent;
    public EventBinding(Action onEventNoArgs) => this.onEventNoArgs = onEventNoArgs;

    public EventBinding(Action<PlayerData> playerReconnected)
    {
        this.playerReconnected = playerReconnected;
    }

    // These methods are now differentiated by their parameter types
    public void AddNoArgs(Action onEvent) => onEventNoArgs += onEvent;
    public void RemoveNoArgs(Action onEvent) => onEventNoArgs -= onEvent;

    public void AddWithArgs(Action<T> onEvent) => this.onEvent += onEvent;
    public void RemoveWithArgs(Action<T> onEvent) => this.onEvent -= onEvent;
}
