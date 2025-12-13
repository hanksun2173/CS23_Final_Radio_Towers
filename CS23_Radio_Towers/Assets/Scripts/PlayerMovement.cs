using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float Horizontal;
    public float jumpForce = 10f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float coyoteTime = 0.2f;
    public float jumpBufferTime = 0.2f;
    public int extraJumpsValue = 2;
    
    private GroundCheck groundCheck;
    
    private Vector2 moveDirection;
    private float coyoteTimeCounter;
    private float jumpBufferCounter;

    private Rigidbody2D rb;
    // true when the player is facing right (matches positive localScale.x)
    private bool FaceRight = true;
    private int extraJumps;
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 10f;
    private float dashingTime = 0.2f;
    private float dashCoolDown = 1f;

    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource footstepSource;
    [SerializeField] private AudioClip[] footstepClips;
    [SerializeField] private float stepInterval = 0.4f;

private float stepTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        groundCheck = GetComponent<GroundCheck>();
        extraJumps = extraJumpsValue;
        
        if (groundCheck == null)
        {
            Debug.LogError("[PlayerMovement] GroundCheck component not found! Please add GroundCheck script to the player.");
        }
    }

    // Update is called once per frame
    void Update() {
        Horizontal = Input.GetAxisRaw("Horizontal");

        if (isDashing)
        {
            return;
        }

        float moveInput = Input.GetAxis("Horizontal");
        // use the standard Rigidbody2D.velocity property
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // Handle coyote time (grace period after leaving ground)
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

        // Handle jump buffering (pressing jump slightly before landing)
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        // Improved jump logic with coyote time and buffering
        if (jumpBufferCounter > 0f)
        {
            if (coyoteTimeCounter > 0f)
            {
                // Ground jump (including coyote time)
                Jump();
                jumpBufferCounter = 0f;
                coyoteTimeCounter = 0f;
            }
            else if (extraJumps > 0)
            {
                // Air jump
                Jump();
                extraJumps--;
                jumpBufferCounter = 0f;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
        
        playerTurn();
        HandleMovement();

    }
    
    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    private void FixedUpdate() {
        ApplyBetterJumpPhysics();
    }
    
    private void ApplyBetterJumpPhysics()
    {
        if (rb.linearVelocity.y < 0)
        {
            // Falling - apply stronger gravity for snappier landings
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !(Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow)))
        {
            // Rising but not holding jump - apply stronger gravity for variable jump height
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

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
    
    private void playerTurn()
    {
        // Flip when input direction doesn't match facing direction
        if ((Horizontal > 0 && !FaceRight) || (Horizontal < 0 && FaceRight))
        {
            FaceRight = !FaceRight;
        }
        // Use SpriteRenderer.flipX for correct animation direction
        var sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.flipX = FaceRight;
        }
       
    }

    private void HandleMovement() {
        if (Horizontal != 0) {
            animator.SetBool("isRunning", true);
        } else {
            animator.SetBool("isRunning", false);
        }

        // Set isJump to true if player is not grounded (jumping or falling)
        bool isGrounded = groundCheck != null ? groundCheck.IsGrounded() : false;
        animator.SetBool("isJump", !isGrounded);
        animator.SetBool("isDash", isDashing);

        
    }

    void PlayFootstep()
    {
        if (footstepClips.Length == 0) return;
        AudioClip clip = footstepClips[Random.Range(0, footstepClips.Length)];
        footstepSource.PlayOneShot(clip);
    }
}
