using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField]
    private float _damageAmount;

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Check if this debris object has the Debris tag
        if (this.gameObject.CompareTag("Debris"))
        {
            // Check if colliding with player
            var healthController = collision.gameObject.GetComponent<HealthController>();
            if (healthController != null)
            {
                Debug.Log($"[EnemyDamage] Debris dealing {_damageAmount} damage to player");
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
