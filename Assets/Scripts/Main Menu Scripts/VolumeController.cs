using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [SerializeField] Slider VolumeSlider; 
    void Start()
    {
        if (!PlayerPrefs.HasKey("MusicVolume"))
        {
            PlayerPrefs.SetFloat("MusicVolume", 0.4f);
            LoadVolume();
            ChangeVolume();
        }

        else
        {
            LoadVolume();
            ChangeVolume();
        }
    }

    private void LoadVolume()
    {
        VolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume"); 
    }

    public void ChangeVolume()
    {
        AudioListener.volume = VolumeSlider.value;
        Save(); 
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("MusicVolume", VolumeSlider.value); 
    }
}
