using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputHandler))]
public class PlayerController : MonoBehaviour, IPlayerEvents
{
    public enum PlayerState { Locked, CanMove, Dashing }

    #region Properties

    public PlayerState State { get; private set; } = PlayerState.Locked;

    // Current buffer for dash
    public float DashTimer { get; private set; } = 0.0f;
    // Time until dash can be used again
    public float DashReloadTime { get; private set; } = 2.0f;
    public bool IsDashing => DashTimer > 0.0f;

    #endregion

    #region Serialized Fields

    [Header("Collision")]
    [SerializeField] private float minimumVelocityOnCollision = 5;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 30;
    [SerializeField] private float accelerationRate = 5; // How fast the player accelerates
    [SerializeField] private float decelerationRate = 1.25f;

    [Header("Dash")]
    [SerializeField] private float dashForce = 500;
    private float dashDuration = 0.75f;

    private Vector2 inputDir;
    private float currentSpeed = 0f; // Tracks the player's current speed
    #endregion

    private Rigidbody rb;
    private Collider col;

    public event Action onPlayerCollision;

    #region Core Methods

    public void Init(PlayerData data)
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        DashTimer = DashReloadTime;
        ChangeState(PlayerState.Locked);
    }

    public void ChangeState(PlayerState newState)
    {
        State = newState;
        if (rb) rb.isKinematic = newState == PlayerState.Locked;
    }

    public void KillPlayer()
    {
        ChangeState(PlayerState.Locked);
        rb.isKinematic = true;
        col.enabled = false;
    }

    // InputHandler Methods
    public void MoveInput(Vector2 moveDir) => inputDir = moveDir;
    public void OnPlayerDash() => Dash();
    public void OnPlayerSpecial() { }

    #endregion

    #region Physics

    private void Update()
    {
        if (State == PlayerState.Locked) return;
        if (DashTimer > 0) DashTimer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (State == PlayerState.Locked) return;
        ApplyMovement();
    }

    public void AddForce(Vector3 forceDir)
    {
        if (rb) rb.AddForce(forceDir);
    }

    private void OnCollisionEnter(Collision collision) => HandleCollision(collision);

    private void ApplyMovement()
    {
        // Gradually increase speed based on input direction
        if (inputDir.magnitude > 0)
        {
            // Accelerate to moveSpeed based on time and accelerationRate
            currentSpeed = Mathf.MoveTowards(currentSpeed, moveSpeed, accelerationRate * Time.deltaTime);
        }
        else
        {
            // If no input, start decelerating
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, accelerationRate * Time.deltaTime);
        }

        // Apply movement force based on current speed
        Vector3 force = new Vector3(inputDir.x, 0, inputDir.y).normalized * currentSpeed;
        rb.AddForce(force);

        // Deceleration (slows down the player when there's no input)
        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, Vector3.zero, Time.deltaTime * decelerationRate);
    }

    public void Dash()
    {
        DashTimer = DashReloadTime;
        //rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        rb.AddForce(new Vector3(inputDir.x, 0, inputDir.y).normalized * dashForce);
    }

    private void HandleCollision(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        onPlayerCollision?.Invoke();

        PlayerController otherPlayer = collision.gameObject.GetComponent<PlayerController>();
        if (otherPlayer == null) return;

        float playerVelocity = rb.linearVelocity.magnitude;
        float otherVelocity = otherPlayer.rb.linearVelocity.magnitude;
        float impactVelocity = Mathf.Clamp((playerVelocity + otherVelocity), minimumVelocityOnCollision, 1000);

        Vector3 direction = (transform.position - otherPlayer.transform.position).normalized;

        if (playerVelocity > otherVelocity)
        {
            rb.linearVelocity = direction * impactVelocity / 2;
            otherPlayer.rb.linearVelocity = IsDashing ? -direction * impactVelocity : -direction * impactVelocity * 1.2f;
        }
    }
    #endregion
}
