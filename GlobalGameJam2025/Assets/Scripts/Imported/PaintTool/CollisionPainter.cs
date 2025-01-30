using UnityEngine;

public class CollisionPainter : MonoBehaviour{
    public Color paintColor;
    
    public float radius = 1;
    public float strength = 1;
    public float hardness = 1;

    private void OnCollisionStay(Collision other) {
        if (!PaintManager.Instance) return;
        Paintable p = other.collider.GetComponent<Paintable>();
        if(p != null){  
            Vector3 pos = other.contacts[0].point;
            PaintManager.Instance.paint(p, pos, radius, hardness, strength, paintColor);
        }
    }
}
