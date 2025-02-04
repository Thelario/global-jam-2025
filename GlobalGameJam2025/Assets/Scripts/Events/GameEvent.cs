using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameEvent")]
public class GameEvent : ScriptableObject
{
    public List<GameEventListener> allListeners = new List<GameEventListener>();

    public void Raise()
    {
        foreach (GameEventListener listener in allListeners) { listener.OnEventRaised(); }
    }

    public void Register(GameEventListener listener)
    {
        if (!allListeners.Contains(listener)) allListeners.Add(listener);
    }
    public void Unregister(GameEventListener listener)
    {
        if (!allListeners.Contains(listener)) allListeners.Remove(listener);
    }
}
