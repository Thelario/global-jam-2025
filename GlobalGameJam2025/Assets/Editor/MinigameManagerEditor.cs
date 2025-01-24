using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MinigameManager))]
public class MinigameManagerEditor : UnityEditor.Editor
{
    SerializedObject SO;

    private void OnEnable() => SO = new SerializedObject(target);

    public override void OnInspectorGUI()
    {
        MinigameManager man = (MinigameManager)target;

        SO.Update();
        
        using (new GUILayout.VerticalScope(EditorStyles.helpBox))
        {
            GUI.enabled = EditorApplication.isPlaying;
            if (GUILayout.Button("Init & Start Game")) man.InitMinigame();
            if (GUILayout.Button("End Game")) man.EndMinigame();
            GUI.enabled = true;
        }

        DrawDefaultInspector();

        SO.ApplyModifiedProperties();
    }
}
