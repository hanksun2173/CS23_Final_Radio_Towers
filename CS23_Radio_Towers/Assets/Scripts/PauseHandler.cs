using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PauseHandler : MonoBehaviour {

        public static bool GameisPaused = false;
        public GameObject pauseMenuUI;
        // public AudioMixer mixer;
        // public static float volumeLevel = 1.0f;
        // private Slider sliderVolumeCtrl;

        void Awake(){
                DontDestroyOnLoad(gameObject);
                // pauseMenuUI.SetActive(true); // so slider can be set
                // SetLevel (volumeLevel);
                // GameObject sliderTemp = GameObject.FindWithTag("PauseMenuSlider");
                // if (sliderTemp != null){
                //         sliderVolumeCtrl = sliderTemp.GetComponent<Slider>();
                //         sliderVolumeCtrl.value = volumeLevel;
                // }
        }

        void OnEnable() {
                SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnDisable() {
                SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
                // Try multiple ways to find pause menu UI
                GameObject found = null;
                // Method 1: Find by name "Pause Menu UI"
                found = GameObject.Find("PauseMenu");
                
                // Method 2: If not found, try finding by tag "PauseMenu"
                if (found == null) {
                        found = GameObject.FindWithTag("PauseMenu");
                }
                
                // Method 3: If still not found, search for any GameObject with "Pause" in the name
                if (found == null) {
                        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
                        foreach (GameObject obj in allObjects) {
                                if (obj.name.Contains("Pause") && obj.scene == scene) {
                                        found = obj;
                                        break;
                                }
                        }
                }
                
                if (found != null) {
                        pauseMenuUI = found;
                        pauseMenuUI.SetActive(false);
                        Debug.Log("[PauseHandler] Found and assigned pause menu: " + found.name + " in " + scene.name);
                } else {
                        pauseMenuUI = null;
                        Debug.LogWarning("[PauseHandler] No pause menu UI found in scene " + scene.name + ". Checked for 'PauseMenuUI' name, 'PauseMenu' tag, and objects containing 'Pause'.");
                }
                GameisPaused = false;
        }

        void Start(){
                if (pauseMenuUI != null) {
                        pauseMenuUI.SetActive(false);
                }
                GameisPaused = false;
        }

        void Update(){
            if (Input.GetKeyDown(KeyCode.Escape)){
                if (GameisPaused) {
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
            else {
                Resume();
            }
            //NOTE: This added conditional is for a pause button
        }

        public void Resume() {
                if (pauseMenuUI != null) {
                        pauseMenuUI.SetActive(false);
                }
                Time.timeScale = 1f;
                GameisPaused = false;
        }

        // public void SetLevel(float sliderValue){
        //         mixer.SetFloat("MusicVolume", Mathf.Log10 (sliderValue) * 20);
        //         volumeLevel = sliderValue;
        // }
}