using UnityEngine;

public class PinballCylinder : MonoBehaviour
{
    [SerializeField] private float pinballForce;
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent<PlayerController>(out var playerController) != true)
            return;

        Vector3 hitDirection = other.transform.position - other.contacts[0].point;

        playerController.SetLinearVelocity(hitDirection * pinballForce);
    }
}
