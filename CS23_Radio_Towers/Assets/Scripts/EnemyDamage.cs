using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField]
    private float _damageAmount;

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Use the correct casing and generic GetComponent<T>() method
        var playerMovement = collision.gameObject.GetComponent<PlayerPlatformMovement>();
        if (playerMovement != null)
        {
            var healthController = collision.gameObject.GetComponent<HealthController>();
            if (healthController != null)
            {
                healthController.TakeDamage(_damageAmount);
            }
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
