using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MinigameManager))]
public class MinigameManagerEditor : UnityEditor.Editor
{
    SerializedProperty testGameProp;
    private void OnEnable()
    {
        testGameProp = serializedObject.FindProperty("TestingGame");
    }
    public override void OnInspectorGUI()
    {
        //MinigameManager manager = (MinigameManager)target;
        serializedObject.Update();
        if (testGameProp.objectReferenceValue != null)
        {
            GUILayout.Label("Test Game", EditorStyles.centeredGreyMiniLabel);
            EditorGUILayout.HelpBox("Esto sobreescribe el minijuego solo si se lanza el juego" +
                "dedse la escena de Gameplay(esta). Para mas info, mirar AssignGame() en MinigameManager", MessageType.Warning);
            GUILayout.Space(5);
            DrawDefaultInspector();
        }
        else DrawDefaultInspector();
        serializedObject.ApplyModifiedProperties();
    }
}