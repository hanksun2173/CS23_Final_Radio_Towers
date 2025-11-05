using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [Header("Pause Menu Setup")]
    [Tooltip("The pause menu UI GameObject (usually a Panel)")]
    public GameObject pauseMenuUI;
    
    public static bool GameIsPaused = false;
    
    void Start()
    {
        // Make sure pause menu starts hidden
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }
        
        // Ensure game starts unpaused
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
    
    void Update()
    {
        // Handle ESC key for pause/unpause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    
    public void Pause()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            GameIsPaused = true;
            Debug.Log("[PauseMenu] Game paused");
        }
        else
        {
            Debug.LogWarning("[PauseMenu] pauseMenuUI is not assigned!");
        }
    }
    
    public void Resume()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }
        Time.timeScale = 1f;
        GameIsPaused = false;
        Debug.Log("[PauseMenu] Game resumed");
    }
}