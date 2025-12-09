using UnityEngine;

public class DebrisAlert : MonoBehaviour
{
    [SerializeField]
    private GameObject exclamationMarkPrefab;
    
    private float lastAlertTime = 0f;
    private float alertCooldown = 0.5f; // Prevent spam alerts
    private GameObject player; // Cache the player reference

    private void Start()
    {
        // Cache player reference once instead of finding it repeatedly
        player = GameObject.FindWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Prevent alert spam with cooldown
        if (Time.time - lastAlertTime < alertCooldown) return;
        
        // Check if debris entered the trigger zone (more efficient check)
        if (other.gameObject.layer == LayerMask.NameToLayer("FallingDebris"))
        {
            // Generate exclamation mark prefab above player
            if (exclamationMarkPrefab != null)
            {
                Vector3 exclamationPosition = transform.position; // Default to alert object position
                
                if (player != null)
                {
                    exclamationPosition = player.transform.position + new Vector3(0, 2f, 0); // 2 units above player
                }
                
                GameObject exclamationMark = Instantiate(exclamationMarkPrefab, exclamationPosition, Quaternion.identity);
                Destroy(exclamationMark, 2f);
                lastAlertTime = Time.time;
            }
        }
    }
}
