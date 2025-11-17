using UnityEngine;

public class RopePlankAttach : MonoBehaviour
{
    private bool isOnCooldown = false;
    private float cooldownDuration = 0.5f;
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
            
            if (plankCollider != null)
            {
                plankCollider.enabled = false;
            }
            
            // Re-enable after cooldown
            Invoke(nameof(EndCooldown), cooldownDuration);
        }
    }
    
    void EndCooldown()
    {
        isOnCooldown = false;
        
        // Re-enable collider
        if (plankCollider != null)
        {
            plankCollider.enabled = true;
            Debug.Log("[RopePlankAttach] Plank collider re-enabled");
        }
    }
}
