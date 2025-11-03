using UnityEngine;

/// <summary>
/// Simple one-line initializer that disables collisions between two layers at startup.
/// Configure the layer names in the Inspector (defaults: Debris and Ground).
/// </summary>
public class CollisionSetup : MonoBehaviour
{
    [Tooltip("First layer name (e.g. 'Debris')")]
    [SerializeField]
    private string layerAName = "Debris";

    [Tooltip("Second layer name (e.g. 'Ground')")]
    [SerializeField]
    private string layerBName = "Ground";

    [Tooltip("Whether to ignore collisions (true) or enable them (false)")]
    [SerializeField]
    private bool ignore = true;

    private void Awake()
    {
        int a = LayerMask.NameToLayer(layerAName);
        int b = LayerMask.NameToLayer(layerBName);

        if (a < 0 || b < 0)
        {
            Debug.LogWarning($"CollisionSetup: Layer '{layerAName}' or '{layerBName}' not found. No layer collision changes applied.");
            return;
        }

        Physics2D.IgnoreLayerCollision(a, b, ignore);
        Debug.Log($"CollisionSetup: Set IgnoreLayerCollision({layerAName},{layerBName}) = {ignore}");
    }
}
