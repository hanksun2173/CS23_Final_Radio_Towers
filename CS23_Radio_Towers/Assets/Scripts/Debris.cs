using UnityEngine;

/// <summary>
/// Attach this to debris prefabs. When the debris collides with any collider tagged "death",
/// the debris GameObject will be destroyed.
/// </summary>
public class Debris : MonoBehaviour
{
    [Tooltip("Tag that will destroy this debris on contact")]
    [SerializeField]
    private string deathTag = "death";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other == null) return;
        if (other.CompareTag(deathTag)) Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision == null || collision.collider == null) return;
        if (collision.collider.CompareTag(deathTag)) Destroy(gameObject);
    }
}
