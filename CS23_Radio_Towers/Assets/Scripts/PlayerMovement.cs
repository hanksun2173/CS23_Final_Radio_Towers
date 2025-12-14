using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class PlayerMovement : MonoBehaviour
{
    // =============================
    //        Player Settings
    // =============================

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public int extraJumpsValue = 2;
    public float coyoteTime = 0.2f;
    public float jumpBufferTime = 0.2f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    [Header("Dash Settings")]
    private float dashingPower = 10f;
    private float dashingTime = 0.2f;
    private float dashCoolDown = 1f;

    [Header("Animation & Audio")]
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource footstepSource;
    [SerializeField] private AudioClip[] footstepClips;
    [SerializeField] private float stepInterval = 0.4f;

    // =============================
    //        State Variables
    // =============================

    private Rigidbody2D rb;
    private GroundCheck groundCheck;
    private float Horizontal;
    private int extraJumps;
    private float coyoteTimeCounter;
    private float jumpBufferCounter;
    private bool FaceRight = true;
    private bool canDash = true;
    private bool isDashing;
    private float stepTimer;

    // =============================
    //        Unity Methods
    // =============================

    /**
     * Initializes references and sets up jump counters.
     */
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        groundCheck = GetComponent<GroundCheck>();
        extraJumps = extraJumpsValue;
        if (groundCheck == null)
            Debug.LogError("[PlayerMovement] GroundCheck component not found! Please add GroundCheck script to the player.");
    }

    /**
     * Handles input, jump logic, coyote time, jump buffering, and dash input.
     */
    void Update()
    {
        Horizontal = Input.GetAxisRaw("Horizontal");
        if (isDashing) return;

        // Horizontal movement
        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // Coyote time & footsteps
        bool isGrounded = groundCheck != null ? groundCheck.IsGrounded() : false;
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            extraJumps = extraJumpsValue;
            if (Mathf.Abs(Horizontal) > 0.1f && !isDashing)
            {
                stepTimer -= Time.deltaTime;
                if (stepTimer <= 0f)
                {
                    PlayFootstep();
                    stepTimer = stepInterval;
                }
            }
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
            stepTimer = 0f;
        }

        // Jump buffering
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
            jumpBufferCounter = jumpBufferTime;
        else
            jumpBufferCounter -= Time.deltaTime;

        // Jump logic
        if (jumpBufferCounter > 0f)
        {
            if (coyoteTimeCounter > 0f)
            {
                Jump();
                jumpBufferCounter = 0f;
                coyoteTimeCounter = 0f;
            }
            else if (extraJumps > 0)
            {
                Jump();
                extraJumps--;
                jumpBufferCounter = 0f;
            }
        }

        // Dash input
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
            StartCoroutine(Dash());

        playerTurn();
        HandleMovement();
    }

    /**
     * Applies jump force to the player.
     */
    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    /**
     * Applies improved jump physics for variable jump height and snappier falls.
     */
    private void FixedUpdate()
    {
        ApplyBetterJumpPhysics();
    }

    /**
     * Modifies gravity for better jump feel (higher falls, variable jump height).
     */
    private void ApplyBetterJumpPhysics()
    {
        if (rb.linearVelocity.y < 0)
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        else if (rb.linearVelocity.y > 0 && !(Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow)))
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
    }

    /**
     * Handles dash movement and cooldown using a coroutine.
     */
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        float dashDir = FaceRight ? 1f : -1f;
        rb.linearVelocity = new Vector2(dashDir * dashingPower, 0f);
        yield return new WaitForSeconds(dashingTime);
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashCoolDown);
        canDash = true;
    }

    /**
     * Flips the player sprite based on movement direction.
     */
    private void playerTurn()
    {
        if ((Horizontal > 0 && !FaceRight) || (Horizontal < 0 && FaceRight))
            FaceRight = !FaceRight;
        var sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.flipX = FaceRight;
    }

    /**
     * Updates animation states for running, jumping, and dashing.
     */
    private void HandleMovement()
    {
        animator.SetBool("isRunning", Horizontal != 0);
        bool isGrounded = groundCheck != null ? groundCheck.IsGrounded() : false;
        animator.SetBool("isJump", !isGrounded);
        animator.SetBool("isDash", isDashing);
    }

    /**
     * Plays a random footstep sound from the provided clips.
     */
    private void PlayFootstep()
    {
        if (footstepClips.Length == 0) return;
        AudioClip clip = footstepClips[Random.Range(0, footstepClips.Length)];
        footstepSource.PlayOneShot(clip);
    }


}