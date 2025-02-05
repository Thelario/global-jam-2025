using UnityEngine;

public interface IEvent { }

public struct TestEvent : IEvent
{

}

#region Player Events
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

public struct PlayerDash : IEvent { public PlayerData data; }
public struct PlayerSpecial : IEvent { public PlayerData data; }
#endregion