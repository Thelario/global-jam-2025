using UnityEditor;
using UnityEngine;

static class EditorMenus
{
    //LOCK INSPECTOR  |  Ctrl + E
    [MenuItem("Tools/Toggle Inspector Lock %e")]
    static void ToggleInspectorLock()
    {
        ActiveEditorTracker.sharedTracker.isLocked = !ActiveEditorTracker.sharedTracker.isLocked;
        ActiveEditorTracker.sharedTracker.ForceRebuild();
    }

    //PING OBJECT  |  Ctrl + W
    //Selecciona el objeto activo, o sino el lockeado en el inspector
    [MenuItem("Tools/Toggle Inspector Ping %w")]
    static void ToggleInspectorPing()
    {
        if (Selection.activeObject != null)
        {
            // If no locked object, ping the currently selected object
            Object selectedObject = Selection.activeObject;
            EditorGUIUtility.PingObject(selectedObject);
        }
        else
        {
            Object lockedObject = GetLockedInspectorTarget();
            if (lockedObject != null) EditorGUIUtility.PingObject(lockedObject);
        }
    }

    //ENTER FULLSCREEN GAME WINDOW |  Ctrl + G
    private static bool isFullscreen = false;
    [MenuItem("Tools/Toggle Fullscreen Game Window %g")]
    private static void ToggleFullscreen()
    {
        EditorWindow gameView = GetGameView();
        if (gameView == null) return;
        if (!isFullscreen) gameView.maximized = true;
        else gameView.maximized = false;

        isFullscreen = !isFullscreen;
        gameView.Focus();
    }

   
    // FOCUS CARPETA INICIAL EN FAVORITOS   |   Ctrl + F
    [MenuItem("Tools/Focus First Favorite %f")]
    private static void FocusFirstFavoriteFolder()
    {
        //Excepciones que sino peta
        var projectBrowserType = System.Type.GetType("UnityEditor.ProjectBrowser,UnityEditor");
        if (projectBrowserType == null) return;
        var projectBrowser = EditorWindow.GetWindow(projectBrowserType);
        if (projectBrowser == null) return;

        var favoritesField = projectBrowserType.GetField("m_Favorites",
            System.Reflection.BindingFlags.NonPublic |
            System.Reflection.BindingFlags.Instance);
        if (favoritesField == null) return;

        var favorites = favoritesField.GetValue(projectBrowser) as System.Collections.IList;
        if (favorites == null || favorites.Count == 0) return;

        var firstFavorite = favorites[0] as string;
        if (!string.IsNullOrEmpty(firstFavorite))
        {
            var folderObject = AssetDatabase.LoadAssetAtPath<Object>(firstFavorite);
            if (folderObject != null)
            {
                Selection.activeObject = folderObject;
                projectBrowser.Focus();
            }
        }
    }

    #region GETTERS
    private static EditorWindow GetGameView()
    {
        System.Type gameViewType = System.Type.GetType("UnityEditor.GameView,UnityEditor");
        if (gameViewType == null) return null;
        return EditorWindow.GetWindow(gameViewType);
    }
    private static Object GetLockedInspectorTarget()
    {
        var inspectorWindowType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.InspectorWindow");
        var trackerField = inspectorWindowType?.GetField("m_Tracker", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var trackerType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.ActiveEditorTracker");
        var isLockedProperty = trackerType?.GetProperty("isLocked", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

        var inspectorWindows = Resources.FindObjectsOfTypeAll(inspectorWindowType);
        foreach (var inspectorWindow in inspectorWindows)
        {
            var tracker = trackerField?.GetValue(inspectorWindow);
            if (tracker == null || isLockedProperty == null) continue;

            bool isLocked = (bool)isLockedProperty.GetValue(tracker);
            if (isLocked)
            {
                var activeEditorsProperty = trackerType.GetProperty("activeEditors", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                var activeEditors = activeEditorsProperty?.GetValue(tracker) as UnityEditor.Editor[];
                if (activeEditors != null && activeEditors.Length > 0) return activeEditors[0]?.target;
            }
        }
        return null;
    }
    #endregion

}