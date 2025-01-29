using UnityEditor;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    private Transform attachedGbj;
    public void Init(MonoBehaviour attachedObject)
    {
        transform.parent = null;
        attachedGbj = attachedObject.transform;
    }

    void LateUpdate()
    {
        if(attachedGbj)
        {
            transform.position = attachedGbj.position;
        }
        else Destroy(this.gameObject);
    }
}
[CustomEditor(typeof(PlayerFollow))]
public class PlayerFollowEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("This will de-attach on Play Mode!", MessageType.Warning);
        DrawDefaultInspector();
    }
}
