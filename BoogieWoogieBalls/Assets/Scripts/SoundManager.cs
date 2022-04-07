using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource gameAudio;
    public AudioSource bgMusic;

    //Target hit sound clips
    public AudioClip redHitSound;
    public AudioClip blueHitSound;
    public AudioClip greenHitSound;
    public AudioClip pinkHitSound;

    public AudioClip catcherSound; //Bullet catch sound effect
    public AudioClip fireSound; //Player fire sound effect
    public AudioClip targetDestroySound; //Sound effect of a target being removed

    public float soundVolume = 0.1f; //Volume of sound effects
    

    private void Start()
    {
        bgMusic.PlayDelayed(1.0f); //play the background music after 1 second when the game starts
    }
    public void PlayRedHitSound()
    {
        gameAudio.PlayOneShot(redHitSound, soundVolume); //play the red target hit audio clip with the chosen volume
    }
    public void PlayBlueHitSound()
    {
        gameAudio.PlayOneShot(blueHitSound, soundVolume); //play the blue target hit audio clip with the chosen volume
    }
    public void PlayGreenHitSound()
    {
        gameAudio.PlayOneShot(greenHitSound, soundVolume); //play the green target hit audio clip with the chosen volume
    }

    public void PlayPinkHitSound()
    {
        gameAudio.PlayOneShot(pinkHitSound, soundVolume); //play the pink target hit audio clip with the chosen volume
    }
    public void PlayCatcherSound()
    {
        gameAudio.PlayOneShot(catcherSound, soundVolume); //play the bullet catcher audio clip with the chosen volume
    }
    public void PlayFireSound()
    {
        gameAudio.PlayOneShot(fireSound, soundVolume); //play the firing audio clip with the chosen volume
    }
    public void PlayDestroySound()
    {
        gameAudio.PlayOneShot(targetDestroySound, soundVolume); //play the target removal audio clip with the chosen volume
    }
}
