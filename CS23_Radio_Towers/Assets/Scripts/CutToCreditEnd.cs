using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CutToCreditEnd : MonoBehaviour
{
    bool triggered = false;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        Debug.Log("CutToCreditEnd triggered by " + other.name);

        if (other.CompareTag("Player"))
        {
            triggered = true;
            StartCoroutine(LoadCreditsAfterDelay());
        }
    }

    IEnumerator LoadCreditsAfterDelay()
    {
        yield return new WaitForSeconds(7);
        FadeManager fadeManager = FindObjectOfType<FadeManager>();
        if (fadeManager != null) {
            fadeManager.FadeToScene("Credits");
        }
        else {
            SceneManager.LoadScene("Credits");
        }
    }
}
