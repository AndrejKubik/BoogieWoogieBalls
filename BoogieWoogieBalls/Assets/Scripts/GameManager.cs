using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int ammo = 8; //number of bullets left to fire
    public int score; //player score
    public int bulletsOnScreen; //number of active bullets

    public List<GameObject> goalTargets; //list of red targets
    public List<GameObject> hitTargets; //list of hit targets
    public int goalTargetCount; //number of red targets remaining
    public int goalTargetsHit; //number of red targets hit

    public float destroyDelay = 0.1f; //time delay between removing hit targets from the screen
    public bool destroyTargets; //trigger for hit target removal sequence

    public bool gameActive; //the game state
    public bool gamePaused; //the game pause state

    public bool powerUp; //trigger for the power-up aim mode
    public int powerUpShotsLeft; //number of better aim shots remaining

    public bool allBulletsDestroyed; //trigger for when there are no more bullets on the screen

    public PlayerController playerController;
    public SoundManager soundManager;
    public TargetBehaviour targetBehaviour;

    public TextMeshProUGUI bulletsLeft;
    public TextMeshProUGUI scoreText;

    public GameObject victoryMenu;
    public GameObject gameOverMenu;

    public ParticleSystem explosion;

    private void Start()
    {
        Physics.gravity = new Vector3(0, -25, 0); //change gravity

        bulletsLeft.text = "" + ammo; //set the starting value of ammo
        scoreText.text = "" + score; //set the starting value of score

        gameActive = true; //make the game active
        gamePaused = false; //set game status to unpaused at start

        goalTargets = new List<GameObject>(GameObject.FindGameObjectsWithTag("TargetGoal")); //Search the screen for all red targets and put them all in a list
        goalTargetCount = goalTargets.Count; //put the number of main targets into a variable
        //Debug.Log("Number of main targets: " + goalTargetCount); //print out the number of main targets

        hitTargets = new List<GameObject>(); //creating a list for storing hit targets
        destroyTargets = false; //hit targets removal is inactive
        goalTargetsHit = 0; //set the number of red targets hit to 0

        powerUp = false; //set the power-up state to inactive

        bulletsOnScreen = 0; //set the number of active bullets to 0 

        allBulletsDestroyed = false; //set the trigger for when all bullets are removed to inactive by default

        powerUpShotsLeft = 0; //set the number of power-up shots to 0 at start
    }
    private void Update()
    {
        if(ammo < 0 && !(goalTargetCount <= 0)) //when player runs out of ammo and misses the catcher(ammo goes bellow 0)
        {
            gameActive = false; //set the game to inactive state

            gameOverMenu.SetActive(true); //show game over menu
        }
        if(goalTargetCount <= 0) //if there are no more red targets on the screen
        {
            gameActive = false; //set the game to inactive state

            victoryMenu.SetActive(true); //show the victory menu
        }

        if(bulletsOnScreen == 0) //if there is no more bullets on the screen
        {
            allBulletsDestroyed = true; //activate the trigger for all bullets being removed so that the target removal can begin
        }

        if(powerUpShotsLeft == 0) //if there is no powered up shots left
        {
            powerUp = false; //disable the powerup
        }

        if (destroyTargets) //when hit targets removal sequence is activated
        {
            StartCoroutine(DestroyTargets(destroyDelay)); //start removing the hit targets with a chosen time delay
        }
    }
    public IEnumerator DestroyTargets(float delay)
    {
        destroyTargets = false; //reset the hit targets removal trigger

        for (int i = 0; i < hitTargets.Count; i++) //for every hit target in the list
        {
            yield return new WaitForSeconds(delay); //wait for input delay in seconds
            Destroy(hitTargets[i].gameObject); //destroy the hit target
            soundManager.PlayDestroySound(); //play the destroying sound effect
            yield return new WaitForEndOfFrame(); //wait 1 frame

            if(i < hitTargets.Count - 1) //when the loop reaches the end
            {
                yield return null; //close the loop
            }
        }
        goalTargetCount -= goalTargetsHit; //reduce the number of remaining red targets by the number of targets hit
        goalTargetsHit = 0; //reset the number of hit red targets
        playerController.canAim = true; //allow the player to aim again
        playerController.canFire = true; //allow the player to fire again
        hitTargets.Clear(); //clear the list of hit targets
    }
}
