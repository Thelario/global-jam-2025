using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public enum PlayerState
    {
        Waiting,
        CanMove
    }
    private void OnEnable()
    {
        MinigameManager manager = MinigameManager.Instance;
        manager.OnMinigameStart += () => ChangeState(PlayerState.CanMove);
        manager.OnMinigameEnd += () => ChangeState(PlayerState.Waiting);
    }
    private void OnDisable()
    {
        MinigameManager manager = MinigameManager.Instance;
        manager.OnMinigameStart -= () => ChangeState(PlayerState.CanMove);
        manager.OnMinigameEnd -= () => ChangeState(PlayerState.Waiting);
    }
    public void ChangeState(PlayerState newState)
    {
        // No lo quites o gran jacobo tu que estas en los cielos
        // Jacobo Nuestro, que estás en Discord,
        // santificado sea tu codigo;
        // venga a nosotros tu pull;
        // hágase tu voluntad, en la tierra como en el cielo.

        if (this == null)
            return;

         // PlayerData playerData = GetComponent<MultiplayerInstance>().PlayerData;

        if (SceneNav.GetCurrentScene() != "EarnPoints")
        {
            rb.isKinematic = newState == PlayerState.Waiting;
        }
        playerState = newState;
    }

    private PlayerState playerState = PlayerState.Waiting;
    [HideInInspector] public int playerIndex;

    [Header("References")]
    [SerializeField] Transform playerFollow;

    [Header("Collision")]
    [SerializeField] float minimunVelocityOnCollision = 5;

    [Header("Movement")]
    [SerializeField] float movementForce = 5;
    float _movementForceInit;
    //[SerializeField] float maxVelocity = 5;


    // Dash
    [Header("Dash")]
    [SerializeField] float dashForce = 500;
    [SerializeField] float dashTime = .1f;
    [SerializeField] Image dashRecharge;
    [SerializeField] float dashDelayTime = 2;
    float dashDelayTimer;
    bool canUseDash = true;

    Vector2 dashDirection;
    bool dashing;

    // Jump
    [Header("Jump")]
    [SerializeField] float jumpForce = 500;
    float smoothTime = .1f;
    private Vector3 m_Velocity = Vector3.zero;
    bool canDoubleJump;
    float jumpRemember = .2f;
    float jumpRememberTimer = -1;

    // Ground checker
    [Header("Ground check")]
    [SerializeField] Transform groundCheck_tr;
    [SerializeField] LayerMask groundLayer;
    [HideInInspector] public bool onGround;
    float groundCheck_radius = .2f;
    float groundRemember = .2f;
    float groundRememberTimer = -1;

    [HideInInspector] public bool stunned;

    private bool _doRollVolume = true;

    public event Action<Collision> OnCollisionEntered;

    // Components
    Rigidbody rb;
    Animator anim;
    BubbleScript bubbleScript;

    // Movement
    Vector2 movementInput;

    public void SetMovementForceMultiplier(float multiplier)
    {
        movementForce *= multiplier;
    }

    public void ResetMovementForce()
    {
        movementForce = _movementForceInit;
        print("entre");
    }

    public void EnableRollVolume(bool enable)
    {
        _doRollVolume = enable;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        anim = GetComponentInChildren<Animator>();
        bubbleScript = GetComponentInChildren<BubbleScript>();

        playerFollow.name = gameObject.name + "_PlayerFollow";
        playerFollow.parent = null;
        _doRollVolume = true;

        dashDelayTimer = dashDelayTime;
        _movementForceInit = movementForce;
        SetMovementForceMultiplier(1f);


        // Set up character variables

        //speed = thisCharacterActions.characterData.speed;
        //jumpForce = thisCharacterActions.characterData.jumpForce;
    }

    void Update()
    {
        if (SceneNav.GetCurrentScene() != "EarnPoints")
        {
            if (playerState == PlayerState.Waiting) return;
        }

        JumpCheck();

        playerFollow.transform.position = transform.position;

        UpdateDashDelay();

        // Limitar velocidad descendente en caso de estar stuneado
        //if (!stunned && !ignoreVelocityLimit)
        //{
        //    if (rb.linearVelocity.y < -15)
        //        rb.linearVelocity = new Vector2(rb.linearVelocity.x, -15);
        //}

        //// Update animator
        //if (input_hor > .1f || input_hor < -.1f)
        //    anim.SetFloat("HorVel", Mathf.Abs(rb.velocity.x));
        //else
        //    anim.SetFloat("HorVel", 0);


        //anim.SetFloat("VerVel", rb.velocity.y);

        //anim.SetBool("OnGround", onGround);

        if (_doRollVolume == false)
            return;

        float volumeModifier = Mathf.Clamp01(rb.linearVelocity.sqrMagnitude / 500);
        SoundManager.Instance.PlaySound(Sound.BubbleRoll, volumeModifier);
    }

    void UpdateDashDelay()
    {
        if (dashDelayTimer >= dashDelayTime * 0.9f)
        {
            canUseDash = true;
        }
        dashDelayTimer += Time.deltaTime;
        dashRecharge.fillAmount = dashDelayTimer / dashDelayTime;
    }


    #region Input

    public virtual void Jump_Input(InputAction.CallbackContext context)
    {
        if (context.started)
            jumpRememberTimer = jumpRemember;

        if (context.canceled && rb.linearVelocity.y > 0)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * .55f);
    }

    public virtual void Dash_Input(InputAction.CallbackContext context)
    {
        if (context.started && canUseDash)
        {
            canUseDash = false;
            dashDelayTimer = 0;

            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
            dashDirection = movementInput;
            rb.AddForce(TwoToThreeVector(dashDirection) * dashForce);

            dashing = true;
            StartCoroutine(DashTime());
        }
    }

    IEnumerator DashTime()
    {
        yield return new WaitForSeconds(dashTime);
        dashing = false;
    }


    #endregion

    #region Movility

    public void MoveInput(InputAction.CallbackContext context)
    {
        movementInput.x = context.ReadValue<Vector2>().x;
        movementInput.y = context.ReadValue<Vector2>().y;
    }

    public void LookInput(InputAction.CallbackContext context)
    {
        //input_hor = context.ReadValue<Vector2>().x;
        //input_ver = context.ReadValue<Vector2>().y;
    }

    void FixedUpdate()
    {
        if (SceneNav.GetCurrentScene() != "EarnPoints")
            if (playerState == PlayerState.Waiting) return;

        Move();

        CheckGround();
    }

    void Move()
    {
        float usableInputHor = movementInput.x;
        float usableInputVer = movementInput.y;

        //if ((attackingMoveDelay && onGround) || stunned)
        //{ realInputHor = 0; realInputVer = 0; }

        //if (!stunned)
        //{
        Vector3 currentMovementForce = new Vector3(usableInputHor, 0, usableInputVer) * movementForce * Time.deltaTime;
        // And then smoothing it out and applying it to the character
        //rb.linearVelocity = Vector3.SmoothDamp(rb.linearVelocity, targetVelocity, ref m_Velocity, smoothTime);
        rb.AddForce(currentMovementForce);

        //Debug.Log("rb.linearVelocity = " + rb.linearVelocity);

        // Frenar la bola
        if (!dashing)
        {
            //if (rb.linearVelocity.magnitude > maxVelocity)
            //    //rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, rb.linearVelocity.normalized * maxVelocity, Time.deltaTime);
            //    rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, Vector3.zero, Time.deltaTime * rb.linearVelocity.magnitude / 2);

            //else if (movementInput.magnitude < .1f)
            //    rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, Vector3.zero, Time.deltaTime);

            if (movementInput.magnitude < .1f)
                rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, Vector3.zero, Time.deltaTime);
        }

        //}
        //else
        //{
        //    Vector3 targetVelocity = new Vector2(0, rb.linearVelocity.y);
        //    // And then smoothing it out and applying it to the character
        //    rb.linearVelocity = Vector3.SmoothDamp(rb.linearVelocity, targetVelocity, ref m_Velocity, .5f);
        //}
    }

    private void OnDrawGizmos()
    {
        if (groundCheck_tr != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck_tr.position, groundCheck_radius);
        }
    }

    void CheckGround()
    {
        Collider[] colliders = Physics.OverlapSphere(groundCheck_tr.position, groundCheck_radius, groundLayer);

        if (colliders.Length == 0)
            onGround = false;
        else
        {
            onGround = true;
            canDoubleJump = true;

            groundRememberTimer = groundRemember;
        }

        onGround_Remember = onGround;
    }
    bool onGround_Remember;

    void JumpCheck()
    {
        jumpRememberTimer -= Time.deltaTime;
        groundRememberTimer -= Time.deltaTime;

        if (rb.linearVelocity.y > 4)
            groundRememberTimer = 0;

        if (stunned) return;

        if (jumpRememberTimer > 0 && groundRememberTimer > 0)
        {
            jumpRememberTimer = 0;
            groundRememberTimer = 0;

            onGround = false;
            //rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            rb.AddForce(new Vector3(0f, jumpForce, 0f));
        }

        if (jumpRememberTimer > 0 && canDoubleJump)
        {
            jumpRememberTimer = 0;

            canDoubleJump = false;
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            rb.AddForce(new Vector2(0f, jumpForce));
        }
    }

    #endregion

    Vector3 TwoToThreeVector(Vector2 vector)
    {
        return new Vector3(vector.x, 0, vector.y);
    }

    Vector2 ThreeToTwoVector(Vector3 vector)
    {
        return new Vector2(vector.x, vector.z);
    }

    Vector3 IgnoreY(Vector3 vector)
    {
        return new Vector3(vector.x, 0, vector.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Aplicar velocidades de impacto
        PlayerController otherPlayer = collision.gameObject.GetComponent<PlayerController>();

        //GameplayMultiplayerManager.Instance.GetAllPlayers();

        if (otherPlayer != null)
        {
            // El player que tenga mas velocidad resuelve las velocidades
            float playerVelocity = GetVelocityMagnitude();
            float otherPlayerVelocity = otherPlayer.GetVelocityMagnitude();

            float velocityMid = (playerVelocity + otherPlayerVelocity);

            Vector3 dir = transform.position - otherPlayer.transform.position;
            dir.Normalize();

            float resultVelocity = Mathf.Clamp(velocityMid, minimunVelocityOnCollision, 1000);

            // El jugador que va mas rapido se encarga de resolver la colision
            if (playerVelocity > otherPlayerVelocity)
            {
                SetLinearVelocity(dir * resultVelocity);
                otherPlayer.SetLinearVelocity(-dir * resultVelocity);
            }
        }

        if (bubbleScript != null)
            bubbleScript.Collision(collision);

        OnCollisionEntered?.Invoke(collision);
    }

    public void AddLinearVelocity(Vector3 vector)
    {
        rb.linearVelocity += vector;
    }

    public void SetLinearVelocity(Vector3 linearVelocity)
    {
        rb.linearVelocity = linearVelocity;
    }

    public Vector3 GetLinearVelocity()
    {
        return rb.linearVelocity;
    }

    public float GetVelocityMagnitude()
    {
        return rb.linearVelocity.magnitude;
    }
}
