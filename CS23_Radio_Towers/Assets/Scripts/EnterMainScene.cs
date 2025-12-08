using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterMainScene : MonoBehaviour
{
    public string mainSceneName = "MainScene";
    
    void Start()
    {

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Load the main scene
            SceneManager.LoadScene(mainSceneName);
        }
    }
}
