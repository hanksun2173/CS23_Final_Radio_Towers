using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveToMinigame : MonoBehaviour
{

    [SerializeField]
    private string sceneToLoad = "EasyPuzzle";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"OnTriggerEnter2D called by: {collision.gameObject.name}, tag: {collision.tag}");
        if (collision.CompareTag("Player"))
        {
            Debug.Log($"Player entered trigger. Attempting to load scene: {sceneToLoad}");
            if (!string.IsNullOrEmpty(sceneToLoad))
            {
                if (Application.CanStreamedLevelBeLoaded(sceneToLoad))
                {
                    SceneManager.LoadScene(sceneToLoad);
                    Debug.Log($"Scene {sceneToLoad} loaded successfully.");
                }
                else
                {
                    Debug.LogError($"Scene '{sceneToLoad}' cannot be loaded. Check build settings and spelling.");
                }
            }
            else
            {
                Debug.LogError("Scene name is empty. Please set sceneToLoad in the inspector.");
            }
        }
    }
}
