using System;
using UnityEngine;

public class Walll : MonoBehaviour
{
    [SerializeField] private float wallForce;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent<PlayerController>(out var playerController) != true)
            return;

        Vector3 hitDirection = other.transform.position - other.contacts[0].point;
        //hitDirection
        
        playerController.SetLinearVelocity(hitDirection * wallForce);
    }
}
