using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MinigameManager))]
public class MinigameManagerEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        GUILayout.Label("Test Game", EditorStyles.centeredGreyMiniLabel);
        EditorGUILayout.HelpBox("Esto sobreescribe el minijuego solo si se lanza el juego" +
            "dedse la escena de Gameplay(esta). Para mas info, mirar AssignGame() en MinigameManager", MessageType.Warning);
        GUILayout.Space(5);
        DrawDefaultInspector();
    }
}