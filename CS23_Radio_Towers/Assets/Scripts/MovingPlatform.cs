using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Platform Movement")]
    public float speed = 2f;
    public Transform[] points;
    private int i;

    [Header("Lever Control")]
    public GameObject leverObject; // Assign in Inspector if platform is lever-controlled
    private Animator leverAnimator;
    private bool leverActivated = false;
    private bool isMoving = false;

    void Start()
    {
        transform.position = points[0].position;
        if (leverObject != null)
        {
            leverAnimator = leverObject.GetComponent<Animator>();
            isMoving = false;
            leverActivated = false;
        }
        else
        {
            isMoving = true; // No lever: move automatically
        }
        Debug.Log($"[MovingPlatform] Initialized - isMoving: {isMoving}, leverAssigned: {leverObject != null}");
    }

    void Update()
    {
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
        if (leverActivated) return;
        leverActivated = true;
        isMoving = true;
        if (leverAnimator != null)
        {
            leverAnimator.SetBool("IsActivated", true);
        }
        Debug.Log("[MovingPlatform] Lever activated! Platform starting to move.");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (gameObject.activeInHierarchy && transform != null)
            {
                try
                {
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
            SafelyUnparentPlayer(collision.transform);
        }
    }

    private void SafelyUnparentPlayer(Transform playerTransform)
    {
        if (playerTransform != null && playerTransform.parent == transform)
        {
            playerTransform.SetParent(null);
            Debug.Log("[MovingPlatform] Player unparented from moving platform");
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
