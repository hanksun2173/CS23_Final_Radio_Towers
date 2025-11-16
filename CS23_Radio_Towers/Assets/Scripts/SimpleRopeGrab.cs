using UnityEngine;

public class SimpleRopeGrab : MonoBehaviour
{
    [Header("Rope Grab Settings")]
    [Tooltip("Force applied when swinging left/right")]
    public float swingForce = 15f;
    
    [Tooltip("Should the player automatically grab ropes on collision?")]
    public bool autoGrab = true;
    
    [Tooltip("Key to manually grab/release ropes")]
    public KeyCode grabKey = KeyCode.Space;
    
    private HingeJoint2D ropeJoint;
    private Rigidbody2D playerRb;
    private GameObject currentRope;
    private bool canGrab = false;
    
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        
        if (playerRb == null)
        {
            Debug.LogError("[SimpleRopeGrab] Player needs a Rigidbody2D component!");
        }
    }
    
    void Update()
    {
        // Manual grab/release with key
        if (Input.GetKeyDown(grabKey))
        {
            if (ropeJoint == null && canGrab && currentRope != null)
            {
                GrabRope(currentRope);
            }
            else if (ropeJoint != null)
            {
                ReleaseRope();
            }
        }
        
        // Apply swing force while holding rope
        if (ropeJoint != null)
        {
            ApplySwingForce();
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"[SimpleRopeGrab] Collision with: {other.name}, Tag: {other.tag}");
        
        // Check if we hit a rope
        if (other.CompareTag("Rope"))
        {
            currentRope = other.gameObject;
            canGrab = true;
            
            Debug.Log($"[SimpleRopeGrab] Found rope: {currentRope.name}");
            
            // Auto-grab if enabled
            if (autoGrab && ropeJoint == null)
            {
                GrabRope(currentRope);
            }
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        // If we leave the rope trigger area, we can't grab it anymore
        if (other.CompareTag("Rope") && other.gameObject == currentRope)
        {
            canGrab = false;
            if (!autoGrab)
            {
                currentRope = null;
            }
        }
    }
    
    void GrabRope(GameObject rope)
    {
        if (rope == null)
        {
            Debug.LogWarning("[SimpleRopeGrab] Trying to grab null rope!");
            return;
        }
        
        Rigidbody2D ropeRb = rope.GetComponent<Rigidbody2D>();
        if (ropeRb == null)
        {
            Debug.LogWarning($"[SimpleRopeGrab] Rope {rope.name} has no Rigidbody2D!");
            return;
        }
        
        if (ropeRb == playerRb)
        {
            Debug.LogWarning("[SimpleRopeGrab] Cannot connect to self!");
            return;
        }
        
        Debug.Log($"[SimpleRopeGrab] Grabbing rope: {rope.name}");
        
        // Create the hinge joint
        ropeJoint = gameObject.AddComponent<HingeJoint2D>();
        ropeJoint.connectedBody = ropeRb;
        ropeJoint.anchor = Vector2.zero;
        ropeJoint.connectedAnchor = rope.transform.InverseTransformPoint(transform.position);
        
        // Configure joint settings
        ropeJoint.enableCollision = false;
        ropeJoint.autoConfigureConnectedAnchor = false;
        
        Debug.Log("[SimpleRopeGrab] Successfully attached to rope!");
    }
    
    void ReleaseRope()
    {
        if (ropeJoint != null)
        {
            Debug.Log("[SimpleRopeGrab] Releasing rope");
            Destroy(ropeJoint);
            ropeJoint = null;
        }
        
        if (!autoGrab)
        {
            currentRope = null;
            canGrab = false;
        }
    }
    
    void ApplySwingForce()
    {
        if (playerRb == null) return;
        
        float horizontalInput = Input.GetAxis("Horizontal");
        if (Mathf.Abs(horizontalInput) > 0.1f)
        {
            Vector2 swingDirection = new Vector2(horizontalInput, 0);
            playerRb.AddForce(swingDirection * swingForce, ForceMode2D.Force);
        }
    }
    
    // Visual feedback in Scene view
    void OnDrawGizmos()
    {
        if (ropeJoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, ropeJoint.connectedBody.transform.position);
        }
        
        if (canGrab && currentRope != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
        }
    }
}