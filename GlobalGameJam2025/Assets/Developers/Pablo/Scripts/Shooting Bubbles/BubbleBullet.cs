using System;
using UnityEngine;

public class BubbleBullet : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float forceMultiplier = 5f;

    private void Update()
    {
        transform.position += transform.right * (moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Vector3 collisionPoint = other.ClosestPoint(transform.position);
        Vector3 distance = (collisionPoint - transform.position).normalized;
        
        other.gameObject.GetComponent<Rigidbody>().AddForce(distance * moveSpeed * forceMultiplier, ForceMode.Impulse);
        gameObject.GetComponent<Rigidbody>().AddForce(-distance * moveSpeed * forceMultiplier, ForceMode.Impulse);
    }
}