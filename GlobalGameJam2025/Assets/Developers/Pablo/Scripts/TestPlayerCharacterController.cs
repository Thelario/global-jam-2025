using UnityEngine;

public class PlayerCharacterController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float movementSpeed = 500f;
    
    [Header("Capsule Height Spring")]
    [SerializeField] private bool debugRaycast = true;
    [SerializeField] private float desiredHeight = 1.5f;
    [SerializeField] private float raycastDistance = 3.0f;
    [SerializeField] private float springForceMultiplier = 2.5f;
    [SerializeField] private LayerMask layerMask;
    
    [Header("Jump")]
    [SerializeField] private float jumpDesiredHeight = 4f;
    [SerializeField] private float desiredHeightDecreaseRate = 2f;
    
    [Header("References")]
    [SerializeField] private Rigidbody rb;
 
    private float _horizontal;
    private float _vertical;
    private float _desiredHeight;

    private void Start()
    {
        _desiredHeight = desiredHeight;
    }

    private void Update()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Jump")) {
            _desiredHeight = jumpDesiredHeight;
        }
    }

    private void FixedUpdate()
    {
        Move();
        AdjustCapsuleHeight();
    }

    private void Move()
    {
        Vector3 direction = new Vector3(_horizontal, 0, _vertical).normalized;
        rb.linearVelocity = direction * (movementSpeed * Time.fixedDeltaTime);
    }

    private void AdjustCapsuleHeight()
    {
        _desiredHeight = Mathf.Clamp(_desiredHeight - desiredHeightDecreaseRate * Time.fixedDeltaTime,
            desiredHeight, _desiredHeight);
        
        // Cast a ray downwards and apply a force to move the capsule until the desired height is reached.

        bool raycastHit = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down),
            out RaycastHit hit, raycastDistance, layerMask);

        if (debugRaycast) {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * raycastDistance,
                raycastHit ? Color.green : Color.red);
        }

        if (raycastHit == false)
            return;
        
        float force = _desiredHeight - hit.distance;

        rb.AddForce(transform.TransformDirection(Vector3.up) * (force * springForceMultiplier));
    }
}
