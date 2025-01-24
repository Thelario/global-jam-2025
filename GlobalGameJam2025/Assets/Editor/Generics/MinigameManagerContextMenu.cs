using UnityEditor;
using UnityEngine;

public class MinigameManagerContextMenu
{
    [MenuItem("GameObject/Init Minigames", false, -1)]
    private static void CheckOrCreateMinigameManager()
    {
        MinigameManager existingManager = Object.FindFirstObjectByType<MinigameManager>();

        if (existingManager)
        {
            Selection.activeGameObject = existingManager.gameObject;
            return;
        }

        GameObject newManagerGO = new GameObject("MinigameManager");
        newManagerGO.AddComponent<MinigameManager>();
        Selection.activeGameObject = newManagerGO;
        
        Debug.Log("Created a new MinigameManager.");

    }
}
