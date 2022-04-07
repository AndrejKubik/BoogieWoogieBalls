using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundVolumeScript : MonoBehaviour
{
    public AudioMixer masterAudioMixer;
    public Slider musicSlider;

    private void Start()
    {
        musicSlider.value = -20f; //set the starting music volume to -20
    }

    private void Update()
    {
        if(musicSlider.value <= -30f) //when the slider value goes bellow -30
        {
            masterAudioMixer.SetFloat("MusicVolume", -80f); //mute the volume totally
        }
    }
    public void SetMasterVolume(float volume)
    {
        masterAudioMixer.SetFloat("MasterVolume", volume); //change the value of master volume slider
    }

    public void SetMusicVolume(float volume)
    {
        masterAudioMixer.SetFloat("MusicVolume", volume); //change the value of music volume slider
    }

    public void SetSFXVolume(float volume)
    {
        masterAudioMixer.SetFloat("SFXVolume", volume); //change the value of sfx volume slider
    }
}
