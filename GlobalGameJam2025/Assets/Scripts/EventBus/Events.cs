using UnityEngine;

public interface IEvent { }

public struct TestEvent : IEvent
{

}


public struct PlayerConnectionEvent : IEvent
{
    public ConnectionType conType;
    public PlayerData data;
}
public enum ConnectionType
{
    Connected,
    Disconnected,
}