using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using UnityEngine.UI;
using TMPro;

public class MainMenuScript : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject settings;

    public AudioSource mainMenuSound;
    public AudioClip menuButtonSound;
    public float soundVolume = 0.1f;

    public AsyncOperation levelLoad;
    public float progress;

    public GameObject loadingBar;
    public Slider loadSlider;
    public TextMeshProUGUI progressText;

    

    public void StartGame()
    {
        mainMenuSound.PlayOneShot(menuButtonSound, soundVolume); //play the button click sound
        loadingBar.SetActive(true); //show the loading bar
        LoadLevel("Level1"); //load the first level of the game
    }

    public async void LoadLevel(string levelName)
    {
        await LoadAsyncly(levelName); //start the async loading of a level and wait for it to finish
        Time.timeScale = 0f; //freeze game time
    }

    public async Task LoadAsyncly(string levelName)
    {
        levelLoad = SceneManager.LoadSceneAsync(levelName); //load the chosen level

        while (!levelLoad.isDone) //until the chosen level is loaded:
        {
            progress = Mathf.Clamp01(levelLoad.progress / 0.9f); //calculate the 0,1 type of the loading progress value instead of 0 - 0.9
            loadSlider.value = Mathf.MoveTowards(loadSlider.value, progress, 0.5f * Time.deltaTime); //change the value of the loading slider to the current progress value smoothly over time
            progressText.text = Mathf.RoundToInt(loadSlider.value * 100f) + "%"; //calculate the progress percentage and set it as text value for the progress text

            await Task.Yield(); //wait for 1 frame before continuing the while loop
        }
    }

    public void OpenSettings()
    {
        mainMenuSound.PlayOneShot(menuButtonSound, soundVolume); //play the button click sound
        mainMenu.SetActive(false); //hide main menu
        settings.SetActive(true); //show settings menu
    }

    public void BackToMain()
    {
        mainMenuSound.PlayOneShot(menuButtonSound, soundVolume); //play the button click sound
        settings.SetActive(false); //hide settings menu
        mainMenu.SetActive(true); //show main menu
    }

    public void ExitGame()
    {
        mainMenuSound.PlayOneShot(menuButtonSound, soundVolume); //play the button click sound
        Application.Quit(); //turn the application off
    }
}
