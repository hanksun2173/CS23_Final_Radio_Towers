using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterTower : MonoBehaviour
{
    public string towerId = "Tower1";
    public string levelSceneName = "RadioTower_LVL1";
    public string mainSceneName = "MainScene";
    
    void Start()
    {
        // Check if this tower has been completed and disable trigger if so
        if (GameHandler.HasTowerBeenCompleted(towerId))
        {
            GetComponent<Collider2D>().isTrigger = false;
            Debug.Log($"[EnterTower] Tower '{towerId}' already completed - trigger disabled");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Set the current tower being played so WinTower knows which to mark as completed
            GameHandler.SetCurrentTower(towerId);
            
            // Load the level scene
            FadeManager fadeManager = FindObjectOfType<FadeManager>();
            if (fadeManager != null)
            {
                fadeManager.FadeToScene(levelSceneName);
            }
            else
            {
                SceneManager.LoadScene(levelSceneName);
            }
        }
    }

    private void OnTriggerEnterMain(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Load the main scene
            FadeManager fadeManager = FindObjectOfType<FadeManager>();
            if (fadeManager != null)
            {
                fadeManager.FadeToScene(mainSceneName);
            }
            else
            {
                SceneManager.LoadScene(mainSceneName);
            }
        }
    }
}
