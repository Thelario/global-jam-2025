using UnityEngine;

public class Rotator : MonoBehaviour
{
    public Vector3 rotAmmount;
    void Update()
    {
        transform.Rotate(rotAmmount);
    }
}
