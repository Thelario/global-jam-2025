using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MinigameData), true)]
public class MinigameBaseEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        MinigameData minigameBase = (MinigameData)target;
        serializedObject.Update();

        SerializedProperty minigameName = serializedObject.FindProperty("minigameName");
        SerializedProperty usesTimer = serializedObject.FindProperty("usesTimer");
        SerializedProperty maxTimer = serializedObject.FindProperty("maxTimer");
        SerializedProperty minigamePrefab = serializedObject.FindProperty("minigamePrefab");

        EditorGUILayout.PropertyField(minigameName);
        GUILayout.Space(10);
        EditorGUILayout.PropertyField(usesTimer);

        // Conditionally show maxTimer field
        if (usesTimer.boolValue)
        {
            EditorGUILayout.PropertyField(maxTimer);
        }

        EditorGUILayout.PropertyField(minigamePrefab);

        serializedObject.ApplyModifiedProperties();
    }
}