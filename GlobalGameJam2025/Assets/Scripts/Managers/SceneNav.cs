using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;


[System.Serializable]
public enum SceneType
{
    MainMenuScene = 0,
    PlayerSelect = 1,
    Gameplay = 2,
    Score = 3
}
public static class SceneNav
{
    private static Fader faderInstance;
    private static bool busy = false;

    public static bool IsGameplay()
    {
        return SceneManager.GetActiveScene().buildIndex == (int)SceneType.Gameplay;
    }
    public static void GoTo(SceneType scene)
    {
        if (busy) return;
        EnsureFader();

        busy = true;
        EnsureBeforeChange(scene);
        
    }
    public static void GoToInmediate(SceneType scene)
    {
        DOTween.KillAll();
        DestroyAllSingletons();
        SceneManager.LoadScene((int)scene, LoadSceneMode.Single);
    }

    //Lo mismo pero esperando un tiempo
    public static void GoToWithDelay(SceneType scene, float delay)
    {
        if (busy) return;
        EnsureFader();
        busy = true;
        ExtensionMethods.StartCoroutine(() => EnsureBeforeChange(scene), 2.0f);
    }
    private static void EnsureBeforeChange(SceneType scene)
    {
        faderInstance.FadeOut(() =>
        {
            DOTween.KillAll();
            DestroyAllSingletons();
            SceneManager.LoadScene((int)scene, LoadSceneMode.Single);
            faderInstance.FadeIn(() => busy = false);
        });
    }

    public static string GetCurrentScene() => SceneManager.GetActiveScene().name;
    public static bool DoSceneExist(string scene) => Application.CanStreamedLevelBeLoaded(scene);

    private static void EnsureFader()
    {
        if (faderInstance == null)
        {
            faderInstance = Object.Instantiate(AssetLocator.Data.Fader);
        }
    }
    private static void DestroyAllSingletons()
    {
        foreach (var singleton in Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None))
        {
            if (singleton.GetType().BaseType?.IsGenericType == true &&
                (singleton.GetType().BaseType.GetGenericTypeDefinition() == typeof(Singleton<>)))
            {
                Object.Destroy(singleton.gameObject);
            }
        }
    }
}
