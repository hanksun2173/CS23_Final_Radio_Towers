using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    public static GameHandler Instance { get; private set; }

    [Header("Overworld Spawn Points (MainScene)")]
    [Tooltip("Ordered spawn positions for MainScene. Index 0 = lose/entry spawn, Index 1 = win spawn.")] 
    public Vector2[] spawnPoints;

    [Tooltip("Current active spawn index (auto applied when MainScene loads)")] 
    [SerializeField] private int currentSpawnIndex = 0;

    [Header("Tower Management")]
    [Tooltip("Tracks which towers have been entered (by tower ID)")]
    public static HashSet<string> enteredTowers = new HashSet<string>();

    // public GameObject Health;
    public static int playerHealth = 6;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log("[GameHandler] Duplicate GameHandler found and destroyed. Existing Instance: " + Instance.name);
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log("[GameHandler] GameHandler Instance set: " + gameObject.name);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Only apply spawn positioning in MainScene
        if (scene.name == "MainScene")
        {
            ApplySpawnPosition();
        }
    }

    private void ApplySpawnPosition()
    {
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("[GameHandler] Cannot apply spawn position. spawnPoints not configured.");
            return;
        }
        if (currentSpawnIndex < 0 || currentSpawnIndex >= spawnPoints.Length)
        {
            Debug.LogWarning($"[GameHandler] currentSpawnIndex {currentSpawnIndex} out of range. Using index 0.");
            currentSpawnIndex = 0;
        }

        // Find the player and position them
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Vector2 spawnPosition = spawnPoints[currentSpawnIndex];
            player.transform.position = spawnPosition;
            Debug.Log($"[GameHandler] Player positioned at spawn index {currentSpawnIndex}: {spawnPosition}");
        }
    }

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
        Debug.Log("[GameHandler] RestartGame() called");
        // Reset all tower states when restarting game
        ResetAllTowers();
        SceneManager.LoadScene("MainMenu");
    }

    public void StartGame(){
        Debug.Log("[GameHandler] StartGame() called");
        // Reset to first spawn point when starting new game
        currentSpawnIndex = 0;
        // Reset all tower states when starting new game
        ResetAllTowers();
        SceneManager.LoadScene("MainScene");
    }

    public void Credits(){
        Debug.Log("[GameHandler] Credits() called");
        SceneManager.LoadScene("Credits");
    }

    public void ReplayLastLevel(){ }

    public void QuitGame()
    {
        Debug.Log("[GameHandler] QuitGame() called");
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    // Set which spawn index should be used when returning to MainScene
    public void SetSpawnIndex(int index)
    {
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("[GameHandler] Cannot set spawn index. spawnPoints not configured.");
            return;
        }
        if (index < 0 || index >= spawnPoints.Length)
        {
            Debug.LogWarning($"[GameHandler] Spawn index {index} out of range (Length={spawnPoints.Length}).");
            return;
        }
        currentSpawnIndex = index;
        Debug.Log($"[GameHandler] currentSpawnIndex set to {currentSpawnIndex} at position {spawnPoints[currentSpawnIndex]}");
    }

    // ===== TOWER MANAGEMENT =====
    
    public static void MarkTowerEntered(string towerId)
    {
        enteredTowers.Add(towerId);
        Debug.Log($"[GameHandler] Tower '{towerId}' marked as entered. Total entered: {enteredTowers.Count}");
    }
    
    public static bool HasTowerBeenEntered(string towerId)
    {
        return enteredTowers.Contains(towerId);
    }
    
    public static void ResetAllTowers()
    {
        enteredTowers.Clear();
        Debug.Log("[GameHandler] All tower states reset");
    }
    
    public static void ResetSpecificTower(string towerId)
    {
        enteredTowers.Remove(towerId);
        Debug.Log($"[GameHandler] Tower '{towerId}' state reset");
    }

}
