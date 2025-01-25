using System;
using UnityEngine;

public class SimpleRotator : MonoBehaviour
{
    [SerializeField] float rotationSpeed;
    [SerializeField] float maxFiuuuum;
    
    private float _fiuuuum = 0f;

    private void Start()
    {
        _fiuuuum = 0f;
    }

    private void OnEnable()
    {
        _fiuuuum = 0f;
    }

    private void Update()
    {
        _fiuuuum += Time.deltaTime;
        _fiuuuum = Mathf.Clamp(_fiuuuum, 0f, maxFiuuuum);
        print(_fiuuuum);
        transform.Rotate(Vector3.up, Time.deltaTime * rotationSpeed * _fiuuuum);
    }
}
