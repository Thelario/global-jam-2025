using UnityEngine;

/// <summary>
/// Base Singleton class that holds a static reference but does not auto-instantiate.
/// </summary>
public abstract class StaticInstance<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    protected static void SetInstance(T instance)
    {
        if (Instance == null)
        {
            Instance = instance;
        }
    }

    protected static void ClearInstance()
    {
        if (Instance != null)
        {
            Instance = null;
        }
    }

    protected virtual void Awake()
    {
        if (Instance == null)
        {
            SetInstance(this as T);
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // Prevent duplicates
        }
    }

    protected virtual void OnDestroy()
    {
        if (Instance == this)
        {
            ClearInstance();
        }
    }
}

/// <summary>
/// Singleton that must be manually added to a scene.
/// It does not auto-instantiate if missing.
/// </summary>
public abstract class Singleton<T> : StaticInstance<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
        base.Awake();
    }
}

/// <summary>
/// Persistent singleton that persists across scenes and auto-instantiates if missing.
/// </summary>
public abstract class PersistentSingleton<T> : Singleton<T> where T : MonoBehaviour
{
    private static bool isQuitting = false;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    protected virtual void OnApplicationQuit()
    {
        isQuitting = true;
    }

    protected override void OnDestroy()
    {
        if (Instance == this) ClearInstance();
    }

    /// <summary>
    /// Ensures an instance exists before accessing it.
    /// </summary>
    public static new T Instance
    {
        get
        {
            if (isQuitting) return null; // Prevents re-creation on quit
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
