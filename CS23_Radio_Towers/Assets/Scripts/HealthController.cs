using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class HealthController : MonoBehaviour
{
    [SerializeField]
    private float _currentHealth;

    [SerializeField]
    private float _maximumHealth;

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
    public void TakeDamage(float damageAmount)
    {
        Debug.Log($"[HealthController] TakeDamage called with amount: {damageAmount}");
        Debug.Log($"[HealthController] Current health: {_currentHealth}, Is invincible: {isInvincible}");
        
        if (_currentHealth == 0)
        {
            Debug.Log("[HealthController] Player already dead, ignoring damage");
            return;
        }

        if (isInvincible)
        {
            Debug.Log("[HealthController] Player is invincible, ignoring damage");
            return;
        }

        _currentHealth -= damageAmount;

        OnHealthChanged.Invoke();

        if (_currentHealth < 0)
        {
            _currentHealth = 0;
        }

        if (_currentHealth == 0)
        {
            OnDied.Invoke();
        }
        else
        {
            OnDamaged.Invoke();
        }
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
        Debug.Log($"[HealthController] TRIGGER collision with: {other.name}, Tag: {other.tag}, Is Trigger: {other.isTrigger}");
        
        if (other == null) return;

        if (isInvincible) 
        {
            Debug.Log("[HealthController] Player is invincible, ignoring damage");
            return;
        }

        // Tag-based lethal objects (instant death)
        if (other.CompareTag("Death"))
        {
            Debug.Log("[HealthController] Death tag detected via trigger - calling OnDied");
            OnDied.Invoke();
            return;
        }

        // Component-based lethal objects (legacy/explicit DeathCollider)
        if (other.GetComponent<DeathCollider>() != null)
        {
            Debug.Log("[HealthController] DeathCollider component detected via trigger - calling OnDied");
            OnDied.Invoke();
        }
        else
        {
            Debug.Log($"[HealthController] Object {other.name} collided but has no Death tag, Debris name, or DeathCollider component");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"[HealthController] PHYSICAL collision with: {collision.collider.name}, Tag: {collision.collider.tag}, Is Trigger: {collision.collider.isTrigger}");
        
        if (collision == null || collision.collider == null) return;
        if (isInvincible) 
        {
            Debug.Log("[HealthController] Player is invincible, ignoring collision damage");
            return;
        }

        var col = collision.collider;
        
        // Death objects (instant kill)
        if (col.CompareTag("Death"))
        {
            Debug.Log("[HealthController] Death tag detected via collision - calling OnDied");
            OnDied.Invoke();
            return;
        }

        // Debris objects (damage handled by EnemyDamage script)
        if (col.name.Contains("Debris"))
        {
            Debug.Log("[HealthController] Debris detected via collision - damage will be handled by EnemyDamage script");
            return;
        }

        // Component-based lethal objects
        if (col.GetComponent<DeathCollider>() != null)
        {
            Debug.Log("[HealthController] DeathCollider component detected via collision - calling OnDied");
            OnDied.Invoke();
        }
    }

    // Add these to catch brief collisions that might be missed
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Death") && !isInvincible)
        {
            Debug.Log($"[HealthController] Death trigger STAYING with: {other.name}");
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Death") && !isInvincible)
        {
            Debug.Log($"[HealthController] Death collision STAYING with: {collision.collider.name}");
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log($"[HealthController] Player health initialized: {_currentHealth}/{_maximumHealth}, Invincible: {isInvincible}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
