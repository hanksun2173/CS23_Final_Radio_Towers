using UnityEngine;
using UnityEngine.SceneManagement;

public class WinTower : MonoBehaviour
{
    [Tooltip("Spawn index in GameHandler to use when player wins (e.g. 1)")] 
    public int winSpawnIndex = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (GameHandler.Instance != null)
            {
                // Mark the current tower as completed since player won
                GameHandler.MarkCurrentTowerCompleted();
                
                // Set spawn point for return to MainScene
                GameHandler.Instance.SetSpawnIndex(winSpawnIndex);
            }
            SceneManager.LoadScene("MainScene");
            // Spawn applied by GameHandler on scene load
        }
    }
}
