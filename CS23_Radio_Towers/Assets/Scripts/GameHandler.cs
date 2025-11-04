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

    // public GameObject Health;
    public static int playerHealth = 6;

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
        SceneManager.LoadScene("MainMenu");
    }

    public void StartGame(){
        SceneManager.LoadScene("MainScene");
    }

    public void Credits(){
        SceneManager.LoadScene("Credits");
    }

    public void ReplayLastLevel(){ }

    public void QuitGame()
    {
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

    private void ApplySpawnIfMainScene(Scene scene)
    {
        if (scene.name != "MainScene") return;
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("[GameHandler] No spawn points configured for MainScene.");
            return;
        }
        // Find player GameObject with tag "Player"
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("[GameHandler] Player GameObject not found. Cannot apply spawn.");
            return;
        }
        player.transform.position = spawnPoints[Mathf.Clamp(currentSpawnIndex, 0, spawnPoints.Length - 1)];
        Debug.Log($"[GameHandler] Player positioned at spawn index {currentSpawnIndex} -> {spawnPoints[currentSpawnIndex]}");
        Time.timeScale = 1f;
        PauseHandler.GameisPaused = false;
    }

    void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) => ApplySpawnIfMainScene(scene);
}
