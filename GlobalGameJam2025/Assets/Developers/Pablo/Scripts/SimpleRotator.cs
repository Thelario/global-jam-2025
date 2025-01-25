using System;
using UnityEngine;

public class SimpleRotator : MonoBehaviour
{
    [SerializeField] float rotationSpeed;
    float fiuuuum = 0f;
    private void Update()
    {
        //fiuuuum = Time.deltaTime;
        
        //transform.Rotate(Vector3.up, Time.deltaTime * rotationSpeed * fiuuuum);
        transform.Rotate(Vector3.up, Time.deltaTime * rotationSpeed);
    }
}
