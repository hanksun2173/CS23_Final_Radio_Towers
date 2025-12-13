using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class HealthController : MonoBehaviour
{
    // Animator for damage animation
    private Animator animator;
    [SerializeField]
    private float _currentHealth;

    [SerializeField]
    private float _maximumHealth;

    [Header("Death Settings")]
    public int deathSpawnIndex = 0; // Spawn index to respawn at when health reaches 0

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private float lastDamageTime = -999f;
    
    [Header("Damage Visual Feedback")]
    public float damageFlashDuration = 0.2f;
    public float damageCooldown = 0.5f;


    public float remainingHealthPercentage
    {
        get
        {
            return _currentHealth / _maximumHealth;
        }
    }

    public bool isInvincible;

    public UnityEvent OnDied;

    public UnityEvent OnDamaged;

    public UnityEvent OnHealthChanged;

    void Start() 
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(float damageAmount)
    {
        if (_currentHealth == 0)
            return;
        if (isInvincible)
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
    
    private System.Collections.IEnumerator FlashDamageColor()
    {
        Color flashColor = new Color(1f, 0f, 0f, 1f);
        spriteRenderer.color = flashColor;
        yield return null;
        yield return new WaitForSeconds(damageFlashDuration);
        spriteRenderer.color = originalColor;
        // Reset damage animation trigger
        if (animator != null)
            animator.SetBool("isDamage", false);
        yield return null;
    }

    public void AddHealth(float amountToAdd)
    {
        if (_currentHealth == _maximumHealth)
        {
            return;
        }

        _currentHealth += amountToAdd;

        OnHealthChanged.Invoke();

        if (_currentHealth > _maximumHealth)
        {
            _currentHealth = _maximumHealth;
        }
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

    // If the player collides with hazard objects, die.
    // This checks for a tag named "Death" or for objects that have a DeathCollider component.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other == null) return;

        if (isInvincible) 
        {
            return;
        }

        // Tag-based lethal objects (instant death)
        if (other.CompareTag("Death"))
        {
            OnDied.Invoke();
            return;
        }

        // Component-based lethal objects (legacy/explicit DeathCollider)
        if (other.GetComponent<DeathCollider>() != null)
        {
            OnDied.Invoke();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision == null || collision.collider == null) return;
        if (isInvincible) 
        {
            return;
        }

        var col = collision.collider;
        
        // Death objects (instant kill)
        if (col.CompareTag("Death"))
        {
            OnDied.Invoke();
            return;
        }

        // Debris objects (damage handled by EnemyDamage script)
        if (col.name.Contains("Debris"))
        {
            return;
        }

        // Component-based lethal objects
        if (col.GetComponent<DeathCollider>() != null)
        {
            OnDied.Invoke();
        }
    }

}
