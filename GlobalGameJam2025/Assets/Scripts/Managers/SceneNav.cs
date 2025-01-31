using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Resources;
using Unity.VisualScripting;
using System.Collections.Generic;

[System.Serializable]
public enum SceneType
{
    MainMenuScene = 0,
    PlayerSelect = 1,
    GameSettings = 2,
    Gameplay = 3,
    Score = 4
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