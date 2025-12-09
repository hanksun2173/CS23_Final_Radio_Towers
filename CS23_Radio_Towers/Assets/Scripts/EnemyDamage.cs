using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField]
    private float _damageAmount;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if this debris object has the Debris tag
        if (this.gameObject.CompareTag("Debris"))
        {
            // Check if triggering with player
            var healthController = other.gameObject.GetComponent<HealthController>();
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
