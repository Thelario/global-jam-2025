using System;
using UnityEngine;

public class SimpleRotator : MonoBehaviour
{
    [SerializeField] float rotationSpeed;
    
    private float _fiuuuum = 0f;
    
    private void Update()
    {
        _fiuuuum = Time.deltaTime;
        transform.Rotate(Vector3.up, Time.deltaTime * rotationSpeed * _fiuuuum);
    }
}
