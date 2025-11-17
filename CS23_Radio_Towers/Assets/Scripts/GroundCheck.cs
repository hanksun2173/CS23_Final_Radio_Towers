using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [Header("Ground Detection")]
    public Transform groundCheckPoint;
    public float groundCheckRadius = 0.5f;
    public LayerMask groundLayer;
    
    private bool isGrounded;
    
    void FixedUpdate()
    {
        // Perform ground check using OverlapCircle
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer);
    }
    
    // Public method for other scripts to check if player is grounded
    public bool IsGrounded()
    {
        return isGrounded;
    }
    
    // Method to check if player was grounded recently (for coyote time)
    public bool WasGroundedRecently(float timeThreshold)
    {
        return isGrounded; // This can be expanded later if needed for more complex coyote time
    }
    
    // Draw the ground check circle in Scene view for debugging
    void OnDrawGizmosSelected()
    {
        if (groundCheckPoint != null)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
        }
    }
}