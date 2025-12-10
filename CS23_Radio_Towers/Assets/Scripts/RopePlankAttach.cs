using UnityEngine;

public class RopePlankAttach : MonoBehaviour
{
    private bool isOnCooldown = false;
    private float cooldownDuration = 1f;
    private Collider2D plankCollider;
    void Start()
    {
        plankCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player") && !isOnCooldown) {
            transform.parent.GetComponent<PlayerRopeGrab>().player = collision.gameObject;
            collision.gameObject.GetComponent<PlayerMovement>().enabled = false;
        }
    }
    
    public void StartCooldown()
    {
        if (!isOnCooldown)
        {
            isOnCooldown = true;

            // Disable all RopePlankAttach colliders in the rope
            RopePlankAttach[] allPlanks = transform.parent.GetComponentsInChildren<RopePlankAttach>();
            foreach (var plank in allPlanks)
            {
                if (plank.plankCollider != null)
                {
                    plank.plankCollider.enabled = false;
                }
                plank.isOnCooldown = true;
            }

            // Re-enable after cooldown
            Invoke(nameof(EndCooldownAll), cooldownDuration);
        }
    }

    // Re-enable all plank colliders after cooldown
    void EndCooldownAll()
    {
        RopePlankAttach[] allPlanks = transform.parent.GetComponentsInChildren<RopePlankAttach>();
        foreach (var plank in allPlanks)
        {
            plank.isOnCooldown = false;
            if (plank.plankCollider != null)
            {
                plank.plankCollider.enabled = true;
            }
        }
        Debug.Log("[RopePlankAttach] All plank colliders re-enabled");
    }
}
