using UnityEngine;

public class GameManagerBridge : MonoBehaviour
{
    // This script acts as a bridge between UI buttons and the persistent GameHandler
    // Place this script on any GameObject in scenes that have buttons
    
    [Header("Optional Pause Menu Reference")]
    [Tooltip("Assign the PauseMenu script if this scene has pause functionality")]
    public PauseMenu pauseMenu;
    
    void Start()
    {
        
        
        // Additional debugging
        if (GameHandler.Instance == null)
        {
            GameHandler[] allGameHandlers = FindObjectsByType<GameHandler>(FindObjectsSortMode.None);
        }
    }
    
    // Helper method to ensure GameHandler exists
    private bool EnsureGameHandler()
    {
        if (GameHandler.Instance != null)
            return true;
            
        Debug.LogWarning("[GameManagerBridge] GameHandler.Instance is null! Make sure to start the game from Main Menu scene, or the GameHandler hasn't been created yet.");
        return false;
    }
    
    // ===== MAIN MENU BUTTONS =====
    
    public void StartGame()
    {
        if (EnsureGameHandler())
            GameHandler.Instance.StartGame();
    }
    
    public void Credits()
    {
        if (EnsureGameHandler())
            GameHandler.Instance.Credits();
    }
    
    public void QuitGame()
    {
        if (EnsureGameHandler())
            GameHandler.Instance.QuitGame();
    }
    
    public void RestartGame()
    {
        if (EnsureGameHandler())
            GameHandler.Instance.RestartGame();
    }
    
    // ===== PAUSE MENU BUTTONS =====
    
    public void Resume()
    {
        PauseMenu targetPauseMenu = pauseMenu;
        if (targetPauseMenu == null)
        {
            targetPauseMenu = FindFirstObjectByType<PauseMenu>();
        }
        
        if (targetPauseMenu != null)
        {
            targetPauseMenu.Resume();
        }
            
    }
    
    public void LoadMainScene()
    {
        PauseMenu targetPauseMenu = pauseMenu;
        if (targetPauseMenu == null)
        {
            targetPauseMenu = FindFirstObjectByType<PauseMenu>();
        }
        
        if (targetPauseMenu != null)
            targetPauseMenu.Resume();
        
        
        if (EnsureGameHandler())
        {
            GameHandler.Instance.SetSpawnIndex(0);
            GameHandler.Instance.StartGame();
        }
    }
}