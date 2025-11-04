using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterTower : MonoBehaviour
{
    public static bool towerEntered = false;
    
    void Start()
    {
        if (towerEntered)
        {
            GetComponent<Collider2D>().isTrigger = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Mark tower as entered (persists across scene loads)
            towerEntered = true;
            
            // Disable the collider immediately
            GetComponent<Collider2D>().isTrigger = false;
            
            // Load the level
            SceneManager.LoadScene("RadioTower_LVL1");
        }
    }
}
