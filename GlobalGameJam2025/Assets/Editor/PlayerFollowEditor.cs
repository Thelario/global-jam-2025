using UnityEditor;

[CustomEditor(typeof(PlayerFollow))]
public class PlayerFollowEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("This will de-attach on Play Mode!", MessageType.Warning);
        DrawDefaultInspector();
    }
}
