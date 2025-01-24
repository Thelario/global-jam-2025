using System;
using UnityEngine;

public class Walll : MonoBehaviour
{
    [SerializeField] private float wallForce;
    private Collider wallCollider;

    private void Start()
    {
        wallCollider = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent<PlayerController>(out var playerController) != true)
            return;
        
        playerController.AddLinearVelocity(transform.right * wallForce);
    }
}
