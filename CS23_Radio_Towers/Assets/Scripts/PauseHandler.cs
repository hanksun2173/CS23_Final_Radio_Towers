using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PauseHandler : MonoBehaviour {

        public static PauseHandler Instance { get; private set; }
        public static bool GameisPaused = false;
        public GameObject pauseMenuUI;
        // public AudioMixer mixer;
        // public static float volumeLevel = 1.0f;
        // private Slider sliderVolumeCtrl;

        void Awake(){
                if (Instance != null && Instance != this)
                {
                        Destroy(gameObject);
                        return;
                }
                Instance = this;
                DontDestroyOnLoad(gameObject);
                
                // Make sure the Canvas is also persistent - but only if it's a root object
                if (pauseMenuUI != null)
                {
                        Canvas canvas = pauseMenuUI.GetComponentInParent<Canvas>();
                        if (canvas != null && canvas.transform.parent == null && canvas.gameObject != gameObject)
                        {
                                // Only apply DontDestroyOnLoad if Canvas is a root GameObject
                                DontDestroyOnLoad(canvas.gameObject);
                        }
                        else if (canvas != null && canvas.transform.parent != null)
                        {
                                Debug.LogWarning("[PauseHandler] Canvas is not a root GameObject. Move the Canvas to the root level in the hierarchy for DontDestroyOnLoad to work properly.");
                        }
                }
        }

        void Start(){
                if (pauseMenuUI != null) {
                        pauseMenuUI.SetActive(false);
                        
                        // Ensure proper canvas setup for web builds
                        Canvas canvas = pauseMenuUI.GetComponentInParent<Canvas>();
                        if (canvas != null) {
                                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                                canvas.sortingOrder = 100; // High value to appear on top
                        }
                }
                GameisPaused = false;
                Time.timeScale = 1f; // Ensure game is running
        }

        void Update(){
            if (Input.GetKeyDown(KeyCode.Escape)){
                if(GameisPaused){
                        Resume();
                }
                else{
                        Pause();
                }
            }
        }

        public void Pause() {
            if (!GameisPaused && pauseMenuUI != null) {
                pauseMenuUI.SetActive(true);
                Time.timeScale = 0f;
                GameisPaused = true;
            }
            else if (pauseMenuUI == null) {
                Debug.LogWarning("[PauseHandler] Cannot pause - pauseMenuUI is null!");
            }
            else {
                Resume();
            }
        }

        public void Resume() {
                if (pauseMenuUI != null) {
                        pauseMenuUI.SetActive(false);
                }
                Time.timeScale = 1f;
                GameisPaused = false;
        }

        // Method for pause menu buttons to load MainScene
        public void LoadMainScene() {
                Resume(); // Ensure game is unpaused
                SceneManager.LoadScene("MainScene");
        }

        // public void SetLevel(float sliderValue){
        //         mixer.SetFloat("MusicVolume", Mathf.Log10 (sliderValue) * 20);
        //         volumeLevel = sliderValue;
        // }
}