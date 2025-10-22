using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    // public GameObject Health;
    public static int playerHealth = 5;
    // public static int health_graves_dug = 0;

    void Start()
    {
        UpdateHealth();
    }

    void UpdateHealth()
    {
        // Text HealthText = Health.GetComponent<Text>();
        // HealthText.text = "Health: " + playerHealth;
        // if (playerHealth <= 0)
        // {
        //     SceneManager.LoadScene("GameOverScene");
        // }
    }

    public void AddHealth(int amount) {
        playerHealth += amount;
        // UpdateHealth();
    }

    public void TakeDamage() {
        playerHealth -= 1;
        // UpdateHealth();
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        // GameHandler_PauseMenu.GameisPaused = false;
        playerHealth = 5;
        SceneManager.LoadScene("Menu");
        
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
