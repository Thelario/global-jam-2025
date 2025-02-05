using UnityEngine;

public class HatSway : MonoBehaviour
{
    public Rigidbody rb; // Reference to the rigidbody
    public float swayFactor = 0.5f; // How much the hat sways based on velocity
    public float maxRotation = 10f; // Maximum sway angle
    public float returnSpeed = 5f; // Speed at which the hat returns to original position

    private Quaternion initialRotation; // To store the initial rotation of the hat
    void Start()
    {
        // Store the initial rotation of the hat at the start
        initialRotation = transform.rotation;
    }

    void Update()
    {
        Vector3 velocity = rb.linearVelocity;
        Vector3 swayDirection = -velocity.normalized;
        float swayAmount = Mathf.Clamp(velocity.magnitude * swayFactor, 0f, maxRotation);
        Quaternion targetRotation;
        if (velocity.magnitude > 1f) targetRotation = Quaternion.Euler(0f, swayDirection.x * swayAmount, swayDirection.z * swayAmount);
        else targetRotation = Quaternion.identity;

        transform.rotation = Quaternion.Slerp(transform.rotation, initialRotation * targetRotation, returnSpeed * Time.deltaTime);
    }
}
