using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    // =============================
    //      Fields & Properties
    // =============================

    [Header("Platform Movement")]
    public float speed = 2f;
    public Transform[] points;
    private int i;

    [Header("Lever Control")]
    [SerializeField] private bool isMoving = false;
    public GameObject leverObject;
    private Animator leverAnimator;
    private bool leverActivated = false;

    // =============================
    //      Unity Methods
    // =============================

    private void Start()
    {
        transform.position = points[0].position;
        // If no lever is assigned, platform starts moving immediately
        if (leverObject == null)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        leverActivated = false;
        if (leverObject != null)
        {
            leverAnimator = leverObject.GetComponent<Animator>();
            LeverTrigger leverTrigger = leverObject.GetComponent<LeverTrigger>();
            if (leverTrigger == null)
                leverTrigger = leverObject.AddComponent<LeverTrigger>();
            leverTrigger.movingPlatform = this;
        }
        Debug.Log($"[MovingPlatform] Initialized - isMoving: {isMoving}, leverActivated: {leverActivated}");
    }

    private void Update()
    {
        if (isMoving)
        {
            if (Vector2.Distance(transform.position, points[i].position) < 0.01f)
            {
                i++;
                if (i >= points.Length) i = 0;
            }
            transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);
        }
    }
    
    /// <summary>
    /// Activates the lever, starts platform movement, and triggers lever animation.
    /// </summary>
    public void ActivateLever()
    {
        if (leverActivated) return;
        leverActivated = true;
        isMoving = true;
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

    /// <summary>
    /// Parents the player to the platform on collision for smooth movement.
    /// </summary>
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

    /// <summary>
    /// Unparents the player from the platform when they leave.
    /// </summary>
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            SafelyUnparentPlayer(collision.transform);
    }

    /// <summary>
    /// Safely unparents the player from the platform.
    /// </summary>
    private void SafelyUnparentPlayer(Transform playerTransform)
    {
        if (playerTransform != null)
        {
            try
            {
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

// LeverTrigger is now in its own script (LeverTrigger.cs)
