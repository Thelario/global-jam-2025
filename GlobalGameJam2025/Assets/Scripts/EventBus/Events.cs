using UnityEngine;

public interface IEvent { }

public struct TestEvent : IEvent
{

}

public struct PlayerAddedEvent : IEvent
{
    public PlayerData data;
}
public struct PlayerRemovedEvent : IEvent
{
    public PlayerData data;
}