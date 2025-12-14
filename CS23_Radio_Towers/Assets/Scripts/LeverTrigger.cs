using UnityEngine;

/// <summary>
/// Handles player interaction with a lever to activate a MovingPlatform.
/// Attach this script to the lever GameObject.
/// </summary>
public class LeverTrigger : MonoBehaviour
{
    [HideInInspector] public MovingPlatform movingPlatform;
    private bool playerInRange = false;

    // Checks for player input to activate lever when in range.
    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.L) && movingPlatform != null)
            movingPlatform.ActivateLever();
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
