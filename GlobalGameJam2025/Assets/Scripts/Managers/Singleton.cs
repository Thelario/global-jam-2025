using UnityEngine;

public abstract class StaticInstance<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    protected static void SetInstance(T instance)
    {
        if (Instance == null) Instance = instance;
    }

    protected static void ClearInstance()
    {
        if (Instance != null) Instance = null;
    }

    protected virtual void Awake()
    {
        if (Instance == null) SetInstance(this as T);
        else if (Instance != this) Destroy(gameObject); // Prevent duplicates
    }

    private void OnDestroy() => ClearInstance();
}

public abstract class Singleton<T> : StaticInstance<T> where T : MonoBehaviour
{
}

/// <summary>
/// Persistent singleton that persists across scenes and auto-instantiates if missing.
/// </summary>
public abstract class PersistentSingleton<T> : StaticInstance<T> where T : MonoBehaviour
{
    private static bool isQuitting = false;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject); // Make sure it persists across scenes.
    }

    protected virtual void OnApplicationQuit() => isQuitting = true;

    /// <summary>
    /// Ensures an instance exists before accessing it.
    /// </summary>
    public static new T Instance
    {
        get
        {
            if (isQuitting) return null;

            if (StaticInstance<T>.Instance == null)
            {
                GameObject obj = new GameObject(typeof(T).Name);
                var instance = obj.AddComponent<T>();
                DontDestroyOnLoad(obj);
                SetInstance(instance);
            }
            return StaticInstance<T>.Instance;
        }
    }
}
