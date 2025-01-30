using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class SceneSelectWindow : EditorWindow
{
    [MenuItem("Tools/Scene Viewer")] // Create Window
    public static void OpenWindow()
    {
        SceneSelectWindow window = (SceneSelectWindow)GetWindow(typeof(SceneSelectWindow));
        window.titleContent = new GUIContent("Scene Selector");
        window.Show();
    }

    // Internal Variables
    private int currentScene = 0;
    // Asset References
    private string[] sceneNames = { "Main Menu", "Game Config", "Gameplay", "Ranking" };
    private string[] scenePaths = {
        "Assets/Scenes/MainMenu.unity",
        "Assets/Scenes/GameConfig.unity",
        "Assets/Scenes/Gameplay.unity",
        "Assets/Scenes/Ranking.unity"
    };

    private void OnEnable()
    {
        Selection.selectionChanged += Repaint;
        EditorApplication.update += Repaint; // Always repaint
    }

    private void OnDisable()
    {
        Selection.selectionChanged -= Repaint;
        EditorApplication.update -= Repaint; // Clean up
    }

    private void OnGUI()
    {
        GUIStyle boldStyle = new GUIStyle(GUI.skin.button) { fontStyle = FontStyle.Bold };

        using (new GUILayout.VerticalScope(EditorStyles.helpBox))
        {
            GUILayout.Label("UI/Intro Scenes", EditorStyles.boldLabel);
            for (int i = 0; i < scenePaths.Length; i++)
            {
                GUI.enabled = i != currentScene;
                if (GUILayout.Button(new GUIContent(sceneNames[i]), boldStyle, GUILayout.Height(30)))
                    ChangeLocalScene(i);
                GUILayout.Space(5);
            }
        }
    }

    public void ChangeLocalScene(int value)
    {
        EditorSceneManager.SaveOpenScenes();
        EditorSceneManager.OpenScene(scenePaths[value]);
        RefreshSceneLogic();
    }

    public void RefreshSceneLogic() => currentScene = SceneManager.GetActiveScene().buildIndex;
}
