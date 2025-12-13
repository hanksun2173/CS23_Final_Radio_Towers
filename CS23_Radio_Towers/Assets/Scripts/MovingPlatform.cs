using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Platform Movement")]
    public float speed = 2f;
    public Transform[] points;
    private int i;
    
    [Header("Lever Control")]
    [SerializeField] private bool isMoving = false; // Platform starts still
    public GameObject leverObject; // Reference to the lever GameObject
    
    private Animator leverAnimator;
    private bool leverActivated = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = points[0].position;
        
        // Ensure platform starts still
        isMoving = false;
        leverActivated = false;
        
        // Get lever animator if lever object is assigned
        if (leverObject != null)
        {
            leverAnimator = leverObject.GetComponent<Animator>();
            
            // Add the lever trigger script to the lever object
            LeverTrigger leverTrigger = leverObject.GetComponent<LeverTrigger>();
            if (leverTrigger == null)
            {
                leverTrigger = leverObject.AddComponent<LeverTrigger>();
            }
            leverTrigger.movingPlatform = this;
        }
        
        Debug.Log($"[MovingPlatform] Initialized - isMoving: {isMoving}, leverActivated: {leverActivated}");
    }

    // Update is called once per frame
    void Update()
    {
        // Only move if lever has been activated
        if (isMoving)
        {
            if (Vector2.Distance(transform.position, points[i].position) < 0.01f)
            {
                i++;
                if (i >= points.Length)
                {
                    i = 0;
                }
            }

            transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);
        }
    }
    
    public void ActivateLever()
    {
        if (leverActivated) return; // Prevent multiple activations
        
        leverActivated = true;
        isMoving = true;
        
        // Use boolean parameter for Animator
        if (leverAnimator != null && leverAnimator.runtimeAnimatorController != null)
        {
            leverAnimator.SetBool("IsActivated", true);
            Debug.Log("[MovingPlatform] Setting lever IsActivated to true");
        }
        else
        {
            Debug.LogWarning("[MovingPlatform] Lever Animator or AnimatorController not found!");
        }
        
        Debug.Log("[MovingPlatform] Lever activated! Platform starting to move.");
    }

    // Unity message methods are case-sensitive: use OnCollisionEnter2D / OnCollisionExit2D
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Check if this platform is active and valid before parenting
            if (gameObject.activeInHierarchy && transform != null)
            {
                try
                {
                    // parent the player's transform to this platform so it moves together
                    collision.transform.SetParent(transform);
                    Debug.Log("[MovingPlatform] Player parented to moving platform");
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning($"[MovingPlatform] Could not parent player: {e.Message}");
                }
            }
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Immediately unparent the player safely without using coroutines
            SafelyUnparentPlayer(collision.transform);
        }
    }
    
    private void SafelyUnparentPlayer(Transform playerTransform)
    {
        // Check if the player transform still exists and is valid
        if (playerTransform != null)
        {
            try
            {
                // Check if the player is actually parented to this platform
                if (playerTransform.parent == transform)
                {
                    playerTransform.SetParent(null);
                    Debug.Log("[MovingPlatform] Player safely unparented from platform");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"[MovingPlatform] Could not unparent player: {e.Message}");
            }
        }
    }
}

// Helper class for lever collision detection
public class LeverTrigger : MonoBehaviour
{
    [HideInInspector]
    public MovingPlatform movingPlatform;
    
    private bool playerInRange = false;
    
    private void Update()
    {
        // Only check input if player is in range - more efficient
        if (playerInRange && Input.GetKeyDown(KeyCode.L) && movingPlatform != null)
        {
            movingPlatform.ActivateLever();
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("[LeverTrigger] Player in range - Press L to activate lever");
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("[LeverTrigger] Player left lever range");
        }
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("[LeverTrigger] Player in range - Press L to activate lever");
        }
    }
    
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("[LeverTrigger] Player left lever range");
        }
    }
}
