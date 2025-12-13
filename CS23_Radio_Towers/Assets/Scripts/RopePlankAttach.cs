using UnityEngine;

public class RopePlankAttach : MonoBehaviour
{
    // --- Cooldown State ---
    private bool isOnCooldown = false;
    private float cooldownDuration = 1f;
    private Collider2D plankCollider;

    /**
     * Initialize the plank's collider reference.
     */
    void Start()
    {
        plankCollider = GetComponent<Collider2D>();
    }

    /**
     * Handles player grabbing the rope plank.
     * Disables player movement and sets the player reference in PlayerRopeGrab.
     */
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isOnCooldown)
        {
            transform.parent.GetComponent<PlayerRopeGrab>().player = collision.gameObject;
            collision.gameObject.GetComponent<PlayerMovement>().enabled = false;
        }
    }

    /**
     * Starts cooldown for all planks in the rope, disabling their colliders.
     */
    public void StartCooldown()
    {
        if (isOnCooldown) return;
        isOnCooldown = true;
        RopePlankAttach[] allPlanks = transform.parent.GetComponentsInChildren<RopePlankAttach>();
        foreach (var plank in allPlanks)
        {
            if (plank.plankCollider != null)
                plank.plankCollider.enabled = false;
            plank.isOnCooldown = true;
        }
        Invoke(nameof(EndCooldownAll), cooldownDuration);
    }

    /**
     * Re-enables all plank colliders after cooldown.
     */
    void EndCooldownAll()
    {
        RopePlankAttach[] allPlanks = transform.parent.GetComponentsInChildren<RopePlankAttach>();
        foreach (var plank in allPlanks)
        {
            plank.isOnCooldown = false;
            if (plank.plankCollider != null)
                plank.plankCollider.enabled = true;
        }
        Debug.Log("[RopePlankAttach] All plank colliders re-enabled");
    }
}
