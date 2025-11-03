// DeathCollider is deprecated: player death is handled by HealthController now.
// Keeping this file as a no-op to avoid compile errors from other assets that may reference it.
using UnityEngine;

public class DeathCollider : MonoBehaviour
{
    private void Awake()
    {
        Debug.LogWarning("DeathCollider is deprecated. Player death is handled in HealthController. You can safely remove this component from scene objects.");
    }
}
