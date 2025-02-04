using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public static class EventBusUtil
{
    public static IReadOnlyList<Type> EventTypes { get; set; }
    public static IReadOnlyList<Type> EventBusTypes { get; set; }

#if UNITY_EDITOR
    public static PlayModeStateChange PlayModeState { get; set; }

    [InitializeOnLoadMethod]
    public static void InitializeEditor()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeStateChange;
        EditorApplication.playModeStateChanged += OnPlayModeStateChange;
    }

    static void OnPlayModeStateChange(PlayModeStateChange state)
    {
        PlayModeState = state;
        if(state == PlayModeStateChange.ExitingPlayMode)
        {
            ClearAllBuses();
        }
    }
#endif


    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Initialize()
    {
        EventTypes = PredefinedAssemblyUtils.GetTypes(typeof(IEvent));
        EventBusTypes = InitializeAllBuses();

    }

    static List<Type> InitializeAllBuses()
    {
        List<Type> allBuses = new List<Type>();

        var typedef = typeof(EventBus<>);
        foreach (var type in EventTypes)
        {
            var busType = typedef.MakeGenericType(type);
            allBuses.Add(busType);
            if(GameSettings.LOG_EVENT_BUS) Debug.Log($"INICIALIZADO EventBus<{type.Name}>");
        }
        return allBuses;
    }

    public static void ClearAllBuses()
    {
        if (GameSettings.LOG_EVENT_BUS) Debug.Log("Limpiando EventBuses...");
        for (int i = 0; i < EventBusTypes.Count; i++)
        {
            var busType = EventBusTypes[i];
            var clearMethod = busType.GetMethod(name: "Clear", bindingAttr: BindingFlags.Static | BindingFlags.NonPublic);
            clearMethod.Invoke(null, null);
        }
    }
}
