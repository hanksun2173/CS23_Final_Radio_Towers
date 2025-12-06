using UnityEngine;
public class DebrisIgnoreColliders : MonoBehaviour
{
    
    [SerializeField]
    private string layerAName = "Debris";

    [SerializeField]
    private string layerBName = "Ground";

    [SerializeField]
    private bool ignore = true;

    [SerializeField]
    private string deathLayerName = "Death";

    private int deathLayerIndex = -1;

    // Apply global setting once
    private static bool s_applied = false;

    private void Awake()
    {
        // Apply global layer ignore once
        if (!s_applied)
        {
            int a = LayerMask.NameToLayer(layerAName);
            int b = LayerMask.NameToLayer(layerBName);
            if (a >= 0 && b >= 0)
            {
                Physics2D.IgnoreLayerCollision(a, b, ignore);
                s_applied = true;
                Debug.Log($"DebrisIgnoreColliders: IgnoreLayerCollision({layerAName},{layerBName}) = {ignore}");
            }
            else
            {
                Debug.LogWarning($"DebrisIgnoreColliders: Layer '{layerAName}' or '{layerBName}' not found. No layer-ignore applied.");
            }
        }

        // Cache death layer index per-instance so spawned debris work correctly
        deathLayerIndex = LayerMask.NameToLayer(deathLayerName);
        if (deathLayerIndex < 0)
        {
            Debug.LogWarning($"DebrisIgnoreColliders: Death layer '{deathLayerName}' not found. Debris will not be destroyed by layer checks.");
        }
    }

    // single helper to handle both trigger and collision callbacks
    private void HandleCollision(Collider2D other)
    {
        if (other == null) return;

        // If a Death layer exists and the other object is on it, destroy
        if (deathLayerIndex >= 0 && other.gameObject.layer == deathLayerIndex)
        {
            Destroy(gameObject);
            return;
        }

        // Support explicit DeathCollider component (on the object or a parent)
        if (other.GetComponentInParent<DeathCollider>() != null)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) => HandleCollision(other);

    private void OnCollisionEnter2D(Collision2D collision) => HandleCollision(collision?.collider);
}
