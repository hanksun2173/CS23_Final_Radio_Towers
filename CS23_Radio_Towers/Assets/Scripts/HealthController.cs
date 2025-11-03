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
        if (_currentHealth == 0)
        {
            return;
        }

        if (isInvincible)
        {
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
        if (other == null) return;

        if (isInvincible) return;

        // Tag-based lethal objects
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
        if (isInvincible) return;

        var col = collision.collider;
        if (col.CompareTag("Death"))
        {
            OnDied.Invoke();
            return;
        }

        if (col.GetComponent<DeathCollider>() != null)
        {
            OnDied.Invoke();
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
