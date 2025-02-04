using System;
using System.Collections.Generic;
using UnityEngine;


public static class EventBus<T> where T : IEvent 
{
    static readonly HashSet<IEventBinding<T>> bindings = new HashSet<IEventBinding<T>>();

    public static void Register(EventBinding<T> binding) => bindings.Add(binding);
    public static void DeRegister(EventBinding<T> binding) => bindings.Remove(binding);

    public static void Raise(T @event)
    {
        foreach (var binding in bindings)
        {
            binding.OnEvent.Invoke(@event);
            binding.OnEventNoArgs.Invoke();
        }
    }

    static void Clear()
    {
        if (GameSettings.LOG_EVENT_BUS) Debug.Log($"Limpiando {typeof(T).Name}");
        bindings.Clear();
    }
}
