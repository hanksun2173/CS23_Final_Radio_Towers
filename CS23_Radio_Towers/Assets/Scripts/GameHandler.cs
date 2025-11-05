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
    [Tooltip("Tracks which towers have been completed (by tower ID)")]
    public static HashSet<string> completedTowers = new HashSet<string>();
    
    [Tooltip("Currently active tower being played")]
    public static string currentTowerId = "";

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
            return;
        }
        if (currentSpawnIndex < 0 || currentSpawnIndex >= spawnPoints.Length)
        {
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
            return;
        }
        if (index < 0 || index >= spawnPoints.Length)
        {
            return;
        }
        currentSpawnIndex = index;
    }

    // ===== TOWER MANAGEMENT =====
    
    public static void SetCurrentTower(string towerId)
    {
        currentTowerId = towerId;
        Debug.Log($"[GameHandler] Current tower set to '{towerId}'");
    }
    
    public static void MarkTowerCompleted(string towerId)
    {
        completedTowers.Add(towerId);
        Debug.Log($"[GameHandler] Tower '{towerId}' marked as completed. Total completed: {completedTowers.Count}");
    }
    
    public static void MarkCurrentTowerCompleted()
    {
        if (!string.IsNullOrEmpty(currentTowerId))
        {
            MarkTowerCompleted(currentTowerId);
            currentTowerId = ""; // Clear current tower
        }
        else
        {
            Debug.LogWarning("[GameHandler] No current tower set - cannot mark as completed");
        }
    }
    
    public static bool HasTowerBeenCompleted(string towerId)
    {
        return completedTowers.Contains(towerId);
    }
    
    public static void ResetAllTowers()
    {
        completedTowers.Clear();
        currentTowerId = "";
        Debug.Log("[GameHandler] All tower states reset");
    }
    
    public static void ResetSpecificTower(string towerId)
    {
        completedTowers.Remove(towerId);
        Debug.Log($"[GameHandler] Tower '{towerId}' completion status reset");
    }

}
