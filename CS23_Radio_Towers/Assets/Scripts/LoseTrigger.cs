using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseTrigger : MonoBehaviour
{
    [Tooltip("Spawn index in GameHandler to use when player loses (e.g. 0)")] public int loseSpawnIndex = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (GameHandler.Instance != null)
            {
                GameHandler.Instance.SetSpawnIndex(loseSpawnIndex);
            }
            FadeManager fadeManager = FindObjectOfType<FadeManager>();
            if (fadeManager != null)
            {
                fadeManager.FadeToScene("MainScene");
            }
            else
            {
                SceneManager.LoadScene("MainScene");
            }
        }
    }
}
