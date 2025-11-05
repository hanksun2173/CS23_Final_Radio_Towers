using UnityEngine.SceneManagement;
using UnityEngine;

public class DeathCollider : MonoBehaviour
{
    public int loseSpawnIndex = 0;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (GameHandler.Instance != null)
            {
                GameHandler.Instance.SetSpawnIndex(loseSpawnIndex);
            }
            SceneManager.LoadScene("MainScene");
            // Spawn applied by GameHandler on scene load
        }
    }
}
