using UnityEngine;

/// <summary>
/// SINGLETON QUE SE CREA AUTOMATICAMENTE CUANDO SE LLAMA,
/// SINO EXISTE INSTANCIA. PersistentSingleton PARA NO BORRARSE
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class StaticInstance<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                // Look for an existing instance in the scene
                _instance = FindFirstObjectByType<T>();

                if (_instance == null)
                {
                    GameObject obj = new GameObject(typeof(T).Name);
                    _instance = obj.AddComponent<T>();
                }
            }
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }


    protected static T _instance;

    protected virtual void Awake()
    {
        if (_instance == null) _instance = this as T;
    }

    protected virtual void OnApplicationQuit()
    {
        _instance = null;
        Destroy(gameObject); // Destroy only the component, not the GameObject
    }
    protected virtual void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }
}

public abstract class Singleton<T> : StaticInstance<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this); // Destroy only the component, not the GameObject
            return;
        }
        base.Awake();
    }
}

public abstract class PersistentSingleton<T> : Singleton<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
}
