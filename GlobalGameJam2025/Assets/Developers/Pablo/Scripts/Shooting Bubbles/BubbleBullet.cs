using System;
using UnityEngine;

public class BubbleBullet : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;

    private void Update()
    {
        transform.position += transform.right * (moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<Rigidbody>().AddForce(transform.right * moveSpeed, ForceMode.Impulse);
        Destroy(gameObject);
    }
}