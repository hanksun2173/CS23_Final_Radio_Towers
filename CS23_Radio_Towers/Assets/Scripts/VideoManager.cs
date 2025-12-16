using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string sceneName;
    public AudioSource videoAudioSource; // Assign in inspector
    public GameObject startButtonCanvas; // Assign a Canvas with your button
    public GameObject startButton; // Assign the button GameObject

    void Start()
    {
        // Ensure video and audio are not playing/muted at start
        if (videoPlayer != null)
        {
            videoPlayer.playOnAwake = false;
            videoPlayer.Pause();
            videoPlayer.loopPointReached += OnVideoFinished;
        }
        if (videoAudioSource != null)
            videoAudioSource.mute = true;
        if (startButtonCanvas != null)
            startButtonCanvas.SetActive(true);
    }

    // Call this from your button's OnClick event
    public void StartJourney()
    {
        if (videoAudioSource != null)
            videoAudioSource.mute = false;
        if (videoPlayer != null)
            videoPlayer.Play();
        if (startButton != null)
            startButton.SetActive(false);
        // Optionally, keep the canvas visible for overlays or fade it as well
        // if (startButtonCanvas != null)
        //     startButtonCanvas.SetActive(false);
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        FadeManager fadeManager = FindObjectOfType<FadeManager>();
        if (fadeManager != null)
        {
            fadeManager.FadeToScene(sceneName);
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
