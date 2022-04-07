using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using System.Threading.Tasks;

public class PauseMenuScript : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject settingsMenu;
    public GameObject menuButton;
    public GameObject fireButton;
    public GameManager gameManager;

    public GameObject tutorial0;
    public GameObject[] tutorials;
    public int tutIndex = 0;

    public AudioSource pauseMenuSounds;
    public AudioClip menuButtonSound;
    public float soundVolume = 0.1f;

    public AudioSource bgMusic;

    public PlayerController playerController;
    public GameObject restartChecker;
    public RestartCheck restartCheck;

    public Scene currentScene;

    private void Start()
    {
        restartChecker = GameObject.Find("IsRestartedChecker"); //get the restart checker object reference
        restartCheck = restartChecker.GetComponent<RestartCheck>(); //get the checker's script reference

        currentScene = SceneManager.GetActiveScene(); //put the current scene into a variable for the name check

        if(!restartCheck.isRestarted)  //if the level is not restarted
        {
            tutorial0.SetActive(true); //show the first tutorial window
            playerController.canAim = false; //lock player aim
            playerController.canFire = false; //lock player fire
            restartCheck.isRestarted = true; //block the tutorial from appearing if the game is restarted
            Time.timeScale = 0; //freeze game time
        }
        //else //if the level is restarted
        //{
        //    playerController.canAim = true; //unlock player aim
        //    playerController.canFire = true; //unlock player fire
        //}
    }

    private void Update()
    {
        if(tutorial0.active == true)
        {
            playerController.canAim = false; //lock player aim
            playerController.canFire = false; //lock player fire
        }
    }

    public void PauseGame()
    {
        gameManager.gamePaused = true; //set the game status to paused
        Time.timeScale = 0f; //freeze game time
        pauseMenu.SetActive(true); //pop-out the pause menu panel
        menuButton.SetActive(false); //hide the menu button
        fireButton.SetActive(false); //hide the fire button
        playerController.canAim = false; //lock player aim
    }
    public void ResumeGame()
    {
        pauseMenuSounds.PlayOneShot(menuButtonSound, soundVolume); //play the button click sound
        gameManager.gamePaused = false; //set the game status to unpaused
        pauseMenu.SetActive(false); //hide the pause menu panel
        menuButton.SetActive(true); //show the menu button
        fireButton.SetActive(true); //show the fire button
        playerController.canAim = true; //unlock player aim
        Time.timeScale = 1f; //unfreeze game time
    }
    public async void RestartLevel()
    {
        pauseMenuSounds.PlayOneShot(menuButtonSound, soundVolume); //play the button click sound
        await Task.Delay(400); //wait for 0.4 seconds
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //load the current active scene again
        Time.timeScale = 1f; //unfreeze game time
        playerController.canAim = true; //unlock player aim
        playerController.canFire = true; //lock player fire
        await Task.Delay(400); //wait for 0.4 seconds
    }

    public async void NextLevel()
    {
        pauseMenuSounds.PlayOneShot(menuButtonSound, soundVolume); //play the button click sound
        await Task.Delay(400); //wait for 0.4 seconds
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //load next level
        Time.timeScale = 1f; //unfreeze game time
        playerController.canAim = false; //lock player aim
        playerController.canFire = false; //lock player fire
        restartCheck.isRestarted = false; //reset the level restart check
        await Task.Delay(400); //wait for 0.4 seconds
    }

    public void OpenSettings()
    {
        pauseMenuSounds.PlayOneShot(menuButtonSound, soundVolume); //play the button click sound
        pauseMenu.SetActive(false); //hide the pause menu
        settingsMenu.SetActive(true); //show the settings menu
    }

    public void CloseSettings()
    {
        pauseMenuSounds.PlayOneShot(menuButtonSound, soundVolume); //play the button click sound
        settingsMenu.SetActive(false); //hide the settings menu
        pauseMenu.SetActive(true); //show the pause menu
    }

    public async void ReturnToMainMenu()
    {
        pauseMenuSounds.PlayOneShot(menuButtonSound, soundVolume); //play the button click sound
        await Task.Delay(400); //wait for 0.4 seconds
        SceneManager.LoadScene("MainMenuScene", LoadSceneMode.Single); //load the main menu
    }

    public void ExitGame()
    {
        pauseMenuSounds.PlayOneShot(menuButtonSound, soundVolume); //play the button click sound
        Application.Quit(); //turn the application off
    }

    public void TutorialSystemNext()
    {
        tutorials[tutIndex].SetActive(false); //hide current tutorial window
        tutIndex++; //increment the array index
        tutorials[tutIndex].SetActive(true); //show the next tutorial window
    }

    public void TutorialSystemBack()
    {
        tutorials[tutIndex].SetActive(false); //hide current tutorial window
        tutIndex--; //decrement the array index
        tutorials[tutIndex].SetActive(true); //show the previous tutorial window
    }

    public async void TutorialSystemSkip()
    {
        pauseMenuSounds.PlayOneShot(menuButtonSound, soundVolume); //play the button click sound
        await Task.Delay(400); //wait for 0.4 seconds
        tutorials[tutIndex].SetActive(false); //hide the current tutorial window
        playerController.canAim = true; //unlock player aim
        playerController.canFire = true; //unlock player fire
        Time.timeScale = 1; //resume game time
    }

    public void TutorialReset()
    {
        restartCheck.isRestarted = false;
    }
}
