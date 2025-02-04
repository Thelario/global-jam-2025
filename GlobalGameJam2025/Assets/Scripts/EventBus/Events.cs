using UnityEngine;

public interface IEvent { }

public struct TestEvent : IEvent
{

}

public struct PlayerEvent : IEvent
{
    public PlayerData data;
}