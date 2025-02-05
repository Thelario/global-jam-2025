using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputHandler))]
public class PlayerController : MonoBehaviour
{
    public enum PlayerState { Waiting, CanMove }

    #region Properties

    public PlayerState State { get; private set; } = PlayerState.Waiting;

    private bool CanUseDash => dashDelayTimer >= dashDelayTime * 0.9f;

    #endregion

    #region Serialized Fields

    [Header("Collision")]
    [SerializeField] private float minimumVelocityOnCollision = 5;

    [Header("Movement")]
    [SerializeField] private float movementForce = 5;
    private float _movementForceInit;

    [Header("Dash")]
    [SerializeField] private float dashForce = 500;
    [SerializeField] private float dashTime = 0.1f;
    [SerializeField] private float dashDelayTime = 2;
    private float dashDelayTimer;
    private bool dashing;
    private Vector2 dashDirection;
    private Vector2 movementInput;
    #endregion

    private Rigidbody rb;
    public event Action onPlayerCollision;

    #region Unity Methods

    private void Update()
    {
        if (State == PlayerState.Waiting) return;
        dashDelayTimer += Time.deltaTime;
    }

    public void KillPlayer()
    {
        ChangeState(PlayerState.Waiting);
        rb.isKinematic = true;
        GetComponent<Collider>().enabled = false;
    }

    private void FixedUpdate()
    {
        if (State == PlayerState.Waiting) return;
        Move();
    }

    public void AddForce(Vector3 forceDir)
    {
        if (rb) rb.AddForce(forceDir);
    }

    private void OnCollisionEnter(Collision collision)
    {
        HandleCollision(collision);
    }

    public void ChangeState(PlayerState newState)
    {
        State = newState;
        if (rb) rb.isKinematic = newState == PlayerState.Waiting;
    }

    #endregion

    #region Initialization

    public void Init(PlayerData data)
    {
        rb = GetComponent<Rigidbody>();

        _movementForceInit = movementForce;
        SetMovementForceMultiplier(1f);

        PlayerInput input = GetComponent<PlayerInput>();
        input.SwitchCurrentControlScheme(data.GetDeviceType());
        ChangeState(PlayerState.Waiting);
    }

    #endregion

    #region Movement

    public void MoveInput(Vector2 moveDir)
    {
        movementInput = moveDir;
    }

    private void Move()
    {
        Vector3 force = new Vector3(movementInput.x, 0, movementInput.y) * movementForce * Time.deltaTime;
        rb.AddForce(force);

        if (!dashing && movementInput.magnitude < 0.1f)
        {
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, Vector3.zero, Time.deltaTime);
        }
    }

    public void SetMovementForceMultiplier(float multiplier)
    {
        movementForce *= multiplier;
    }

    public void ResetMovementForce()
    {
        movementForce = _movementForceInit;
    }

    #endregion

    #region Dash

    public void Dash_Input(InputAction.CallbackContext context)
    {
        if (context.started && CanUseDash)
        {
            dashDelayTimer = 0;
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
            dashDirection = movementInput;
            rb.AddForce(dashDirection * dashForce);
            dashing = true;
            StartCoroutine(DashCoroutine());
        }
    }

    private IEnumerator DashCoroutine()
    {
        yield return new WaitForSeconds(dashTime);
        dashing = false;
    }

    #endregion

    #region Collision Handling

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
            otherPlayer.rb.linearVelocity = dashing ? -direction * impactVelocity : -direction * impactVelocity * 1.2f;
        }
    }

    #endregion
}
