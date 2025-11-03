using UnityEngine;

/// <summary>
/// Simplified: disable collisions between two layers at startup.
/// Attach to any GameObject; the change is applied once (static) and affects the physics collision matrix.
/// </summary>
public class DebrisIgnoreColliders : MonoBehaviour
{
    [Tooltip("First layer name (usually the debris layer)")]
    [SerializeField]
    private string layerAName = "Debris";

    [Tooltip("Second layer name (usually the ground layer)")]
    [SerializeField]
    private string layerBName = "Ground";

    [Tooltip("Whether to ignore collisions between the two layers")]
    [SerializeField]
    private bool ignore = true;

    // Ensure we only apply the change once across all instances
    private static bool s_applied = false;

    private void Awake()
    {
        if (s_applied) return;

        int a = LayerMask.NameToLayer(layerAName);
        int b = LayerMask.NameToLayer(layerBName);

        if (a < 0 || b < 0)
        {
            Debug.LogWarning($"DebrisIgnoreColliders: Layer '{layerAName}' or '{layerBName}' not found. No changes applied.");
            return;
        }

        Physics2D.IgnoreLayerCollision(a, b, ignore);
        s_applied = true;
        Debug.Log($"DebrisIgnoreColliders: IgnoreLayerCollision({layerAName},{layerBName}) = {ignore}");
    }
}
