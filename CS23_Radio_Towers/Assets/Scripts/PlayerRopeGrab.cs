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
                
                Vector2 ropeVelocity = rb.linearVelocity;
                
                Vector2 ejectVelocity = ropeVelocity * velocityMultiplier;
                
                // Add upward ejection force
                ejectVelocity.y += ejectionForce;
                
                // Add extra horizontal boost based on rope's swing direction
                float horizontalDirection = Mathf.Sign(ropeVelocity.x);
                ejectVelocity.x += horizontalDirection * horizontalBoost;
                
                playerRb.linearVelocity = ejectVelocity;
                player.GetComponent<PlayerMovement>().enabled = true;
                
                // Start cooldown on the plank
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
