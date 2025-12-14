using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [Header("Ground Detection")]
    public Transform groundCheckPoint;
    public float groundCheckRadius = 0.5f;
    public LayerMask groundLayer;
    
    [Header("Landing Effects")]
    public GameObject dustEffectPrefab; // Drag your dust animation prefab here
    
    [SerializeField] public AudioSource jumpSource;
    [SerializeField] public AudioClip jumpClip;
    [SerializeField] public float minFallSpeedForDust = 2f; // Set a default value as needed
    
    private bool isGrounded;
    private bool wasGroundedLastFrame;
    private Rigidbody2D playerRigidbody;
    private bool hasDustTriggered; // Track if dust already triggered for this landing
    
    
    void Start()
    {
        // Get the player's Rigidbody2D component to check fall speed
        playerRigidbody = GetComponent<Rigidbody2D>();
    }
    
    void FixedUpdate()
    {
        // Store previous grounded state
        wasGroundedLastFrame = isGrounded;
        
        // Perform ground check using OverlapCircle
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer);
        
        // Reset dust trigger when player becomes airborne
        if (!isGrounded && wasGroundedLastFrame)
        {
            hasDustTriggered = false;
        }
        
        // Check if player just landed (wasn't grounded last frame, but is now)
        if (isGrounded && !wasGroundedLastFrame && !hasDustTriggered)
        {
            CheckForLandingEffect();
        }
    }
    
    void CheckForLandingEffect()
    {
        // Only create dust effect if player was falling fast enough
        if (playerRigidbody != null && playerRigidbody.linearVelocity.y <= -minFallSpeedForDust)
        {
            SpawnDustEffect();
            PlayJumpSound();
            hasDustTriggered = true; // Mark that dust has been triggered for this landing
        }
    }
    
    void SpawnDustEffect()
    {
        if (dustEffectPrefab != null)
        {
            // Center the dust effect at the player's position
            Vector3 dustPosition = transform.position;
            
            // Instantiate dust effect like in PaddleMove example
            GameObject dustFX = Instantiate(dustEffectPrefab, dustPosition, Quaternion.identity);
            StartCoroutine(DestroyDustVFX(dustFX));
        }
    }
    
    System.Collections.IEnumerator DestroyDustVFX(GameObject theEffect)
    {
        yield return new WaitForSeconds(1f); // Adjust timing as needed
        Destroy(theEffect);
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

        /**
     * Plays the jump sound effect.
     */
    private void PlayJumpSound()
    {
        if (jumpSource != null && jumpClip != null)
        {
            jumpSource.PlayOneShot(jumpClip);
        }
    }
}