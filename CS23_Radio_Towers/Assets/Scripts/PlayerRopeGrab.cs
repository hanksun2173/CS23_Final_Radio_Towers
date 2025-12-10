using UnityEngine;

public class PlayerRopeGrab : MonoBehaviour
{
    public GameObject player = null;
    public Transform bottomPlank;
    public Rigidbody2D rb;
    public float velocityMultiplier = 1.5f;
    public float ejectionForce = 10f;
    public float horizontalBoost = 5f;
    
    void Start()
    {

    }
    
    void Update()
    {
        if (player != null) {
            player.transform.position = bottomPlank.position;

            if (Input.GetButtonDown("Jump")) {
                Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
                
                // Store original position before ejection
                Vector3 originalPosition = player.transform.position;
                
                Vector2 ropeVelocity = rb.linearVelocity;
                
                Vector2 ejectVelocity = ropeVelocity * velocityMultiplier;
                
                // Add upward ejection force
                ejectVelocity.y += ejectionForce;
                
                // Add extra horizontal boost based on rope's swing direction
                float horizontalDirection = Mathf.Sign(ropeVelocity.x);
                ejectVelocity.x += horizontalDirection * horizontalBoost;
                
                // Re-enable PlayerMovement first
                PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
                if (playerMovement != null)
                {
                    playerMovement.enabled = true;
                }
                
                // Apply velocity after re-enabling movement
                playerRb.linearVelocity = ejectVelocity;
                
                // Ensure player's collider is active
                Collider2D playerCollider = player.GetComponent<Collider2D>();
                if (playerCollider != null)
                {
                    playerCollider.enabled = true;
                }
                

                player.transform.position = new Vector3(originalPosition.x, originalPosition.y + 0.1f, originalPosition.z);
                
                Debug.Log($"[PlayerRopeGrab] Player ejected with velocity: {ejectVelocity} from position: {player.transform.position}");
                

                // Only call StartCooldown on RopePlankAttach
                RopePlankAttach plankScript = bottomPlank.GetComponent<RopePlankAttach>();
                if (plankScript != null)
                {
                    plankScript.StartCooldown();
                }
                
                player = null;
            }
        }
    }
}
