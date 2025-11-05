using UnityEngine;

public class UIBridge : MonoBehaviour
{
    public static UIBridge Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Methods that can be called from UI buttons
    // These will forward calls to the persistent singletons
    
    public void Resume()
    {
        Debug.Log("[UIBridge] Resume() called");
        if (PauseHandler.Instance != null)
        {
            Debug.Log("[UIBridge] PauseHandler.Instance found, calling Resume()");
            PauseHandler.Instance.Resume();
        }
        else
        {
            Debug.LogWarning("[UIBridge] PauseHandler.Instance is null!");
        }
    }
    
    public void LoadMainScene()
    {
        Debug.Log("[UIBridge] LoadMainScene() called");
        if (PauseHandler.Instance != null)
        {
            Debug.Log("[UIBridge] PauseHandler.Instance found, calling LoadMainScene()");
            PauseHandler.Instance.LoadMainScene();
        }
        else
        {
            Debug.LogWarning("[UIBridge] PauseHandler.Instance is null!");
        }
    }
    
    public void StartGame()
    {
        Debug.Log("[UIBridge] StartGame() called");
        if (GameHandler.Instance != null)
        {
            Debug.Log("[UIBridge] GameHandler.Instance found, calling StartGame()");
            GameHandler.Instance.StartGame();
        }
        else
        {
            Debug.LogWarning("[UIBridge] GameHandler.Instance is null!");
        }
    }
    
    public void Credits()
    {
        Debug.Log("[UIBridge] Credits() called");
        if (GameHandler.Instance != null)
        {
            Debug.Log("[UIBridge] GameHandler.Instance found, calling Credits()");
            GameHandler.Instance.Credits();
        }
        else
        {
            Debug.LogWarning("[UIBridge] GameHandler.Instance is null!");
        }
    }
    
    public void QuitGame()
    {
        Debug.Log("[UIBridge] QuitGame() called");
        if (GameHandler.Instance != null)
        {
            Debug.Log("[UIBridge] GameHandler.Instance found, calling QuitGame()");
            GameHandler.Instance.QuitGame();
        }
        else
        {
            Debug.LogWarning("[UIBridge] GameHandler.Instance is null!");
        }
    }
    
    public void RestartGame()
    {
        Debug.Log("[UIBridge] RestartGame() called");
        if (GameHandler.Instance != null)
        {
            Debug.Log("[UIBridge] GameHandler.Instance found, calling RestartGame()");
            GameHandler.Instance.RestartGame();
        }
        else
        {
            Debug.LogWarning("[UIBridge] GameHandler.Instance is null!");
        }
    }
}