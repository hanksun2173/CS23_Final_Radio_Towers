using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterTower : MonoBehaviour
{
    [Header("Tower Configuration")]
    [Tooltip("Unique identifier for this tower (e.g., 'Tower1', 'RadioTower', etc.)")]
    public string towerId = "Tower1";
    
    [Tooltip("Scene to load when entering this tower")]
    public string levelSceneName = "RadioTower_LVL1";
    
    void Start()
    {
        if (GameHandler.HasTowerBeenEntered(towerId))
        {
            GetComponent<Collider2D>().isTrigger = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Mark this specific tower as entered
            GameHandler.MarkTowerEntered(towerId);
            
            // Disable the trigger
            GetComponent<Collider2D>().isTrigger = false;
            
            // Load the level
            SceneManager.LoadScene(levelSceneName);
        }
    }
}
