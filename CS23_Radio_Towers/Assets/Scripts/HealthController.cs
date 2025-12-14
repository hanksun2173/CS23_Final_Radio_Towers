using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class HealthController : MonoBehaviour
{
    // =============================
    //      Fields & Properties
    // =============================

    [Header("Health Settings")]
    [SerializeField] private float _currentHealth;
    [SerializeField] private float _maximumHealth;
    public bool isInvincible;

    [Header("Death Settings")]
    public int deathSpawnIndex = 0;

    [Header("Damage Visual Feedback")]
    public float damageFlashDuration = 0.2f;
    public float damageCooldown = 0.5f;

    // Components
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Color originalColor;

    // Events
    public UnityEvent OnDied;
    public UnityEvent OnDamaged;
    public UnityEvent OnHealthChanged;

    // Utility
    private float lastDamageTime = -999f;

    // Health percentage property
    public float remainingHealthPercentage => _currentHealth / _maximumHealth;

    // =============================
    //      Unity Methods
    // =============================

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Applies damage to the player, triggers animation and events.
    /// </summary>
    public void TakeDamage(float damageAmount)
    {
        if (_currentHealth == 0 || isInvincible)
            return;

        _currentHealth -= damageAmount;

        // Trigger damage animation
        if (animator != null)
            animator.SetBool("isDamage", true);

        StartCoroutine(FlashDamageColor());
        OnHealthChanged.Invoke();

        if (_currentHealth < 0)
            _currentHealth = 0;

        if (_currentHealth == 0)
        {
            if (GameHandler.Instance != null)
                GameHandler.Instance.SetSpawnIndex(deathSpawnIndex);
            SceneManager.LoadScene("MainScene");
        }
        else
        {
            OnDamaged.Invoke();
        }
    }
    
    /// <summary>
    /// Flashes the sprite color and resets damage animation.
    /// </summary>
    private System.Collections.IEnumerator FlashDamageColor()
    {
        Color flashColor = new Color(1f, 0f, 0f, 1f);
        spriteRenderer.color = flashColor;
        yield return null;
        yield return new WaitForSeconds(damageFlashDuration);
        spriteRenderer.color = originalColor;
        if (animator != null)
            animator.SetBool("isDamage", false);
        yield return null;
    }

    /// <summary>
    /// Adds health to the player and clamps to max.
    /// </summary>
    public void AddHealth(float amountToAdd)
    {
        if (_currentHealth == _maximumHealth)
            return;

        _currentHealth += amountToAdd;
        OnHealthChanged.Invoke();
        if (_currentHealth > _maximumHealth)
            _currentHealth = _maximumHealth;
    }

    /// <summary>
    /// Immediately kill the player (respects isInvincible). Invokes OnDied and loads GameOver.
    /// </summary>
    // public void Die()
    // {
    //     if (_currentHealth == 0) return; // already dead
    //     if (isInvincible) return; // can't die while invincible

    //     _currentHealth = 0;
    //     OnHealthChanged.Invoke();
    //     OnDied?.Invoke();

    //     // Load GameOver scene (preserve previous behavior)
    //     SceneManager.LoadScene("GameOver");
    // }

    /// <summary>
    /// Handles instant death on trigger with hazards.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other == null || isInvincible) return;
        if (other.CompareTag("Death"))
        {
            OnDied.Invoke();
            return;
        }
        if (other.GetComponent<DeathCollider>() != null)
        {
            OnDied.Invoke();
        }
    }

    /// <summary>
    /// Handles instant death on collision with hazards.
    /// </summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision == null || collision.collider == null || isInvincible) return;
        var col = collision.collider;
        if (col.CompareTag("Death"))
        {
            OnDied.Invoke();
            return;
        }
        if (col.name.Contains("Debris"))
            return;
        if (col.GetComponent<DeathCollider>() != null)
        {
            OnDied.Invoke();
        }
    }

}
