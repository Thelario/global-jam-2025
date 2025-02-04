using System.Collections.Generic;
using System;

public static class EventBus
{
    private static Dictionary<string, Action<PlayerData>> eventDictionary = new Dictionary<string, Action<PlayerData>>();

    public static void Subscribe(string eventName, Action<PlayerData> listener)
    {
        if (!eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName] = listener;
        }
        else
        {
            eventDictionary[eventName] += listener;
        }
    }

    public static void Unsubscribe(string eventName, Action<PlayerData> listener)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName] -= listener;
            if (eventDictionary[eventName] == null)
            {
                eventDictionary.Remove(eventName);
            }
        }
    }

    public static void Publish(string eventName, PlayerData data)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName]?.Invoke(data);
        }
    }
}