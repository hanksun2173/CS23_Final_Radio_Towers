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
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    public int extraJumpsValue = 2;
    private Vector2 moveDirection;

    private Rigidbody2D rb;
    // true when the player is facing right (matches positive localScale.x)
    private bool FaceRight = true;
    private int extraJumps;
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 10f;
    private float dashingTime = 0.2f;
    private float dashCoolDown = 1f;


    private bool isGrounded;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        rb = GetComponent<Rigidbody2D>();    
        extraJumps = extraJumpsValue;    
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

        if (isGrounded) {
            extraJumps = extraJumpsValue;
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            if (isGrounded) {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
            else if (extraJumps > 0) {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                extraJumps--;

            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
        playerTurn();
        
    }

    private void FixedUpdate() {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
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
            Vector3 theScale = transform.localScale;
            theScale.x *= -1f;
            transform.localScale = theScale;
        }
       
    }
}
