using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    private Transform attachedGbj;
    private bool initialized = false;

    //Lista de objetos a los que poner la posicion con raycast debajo (ej.sombras)
    [SerializeField] private List<Transform> raycastingObjects;
    public void Init(MonoBehaviour attachedObject)
    {
        transform.parent = null;
        attachedGbj = attachedObject.transform;
        initialized = true;
    }

    void LateUpdate()
    {
        if (!initialized) return;
        //Follow
        if(attachedGbj) transform.position = attachedGbj.position;
        else Destroy(this.gameObject);

        //Raycasting
        if (Physics.Raycast(transform.position, -Vector3.up, out RaycastHit hit, 100.0f))
        {
            foreach (Transform t in raycastingObjects)
            {
                //Small offset
                t.position = hit.point + Vector3.up * 0.05f;
            }
        }
        
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
