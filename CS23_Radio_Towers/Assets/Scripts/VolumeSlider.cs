using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;


    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        myMixer.SetFloat("mainvolume", Mathf.Log10(volume)*20);
    }
    
}