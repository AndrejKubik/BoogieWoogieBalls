using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class TargetBehaviour : MonoBehaviour
{
    public GameManager gameManager;
    public SoundManager soundManager;
    public PlayerController playerController;

    //Materials for broken targets
    public Material bonusBroken;
    public Material goalBroken;
    public Material powerUpBroken;
    public Material multiplyBroken;

    private Renderer targetRenderer;

    public ParticleSystem explosion;

    public float timer = 0;
    public GameObject stuckTarget;
    

    private void Start()
    {
        targetRenderer = GetComponent<MeshRenderer>();
    }
    
    private async void OnCollisionEnter(Collision collision)
    {
        if (gameObject.CompareTag("TargetGoal")) //when the bullet hits a red target
        {
            ActivateTarget(gameObject, goalBroken, 5); //change target's material and add 3 to score
            gameManager.goalTargetsHit++; //add 1 to the number of red targets hit
            soundManager.PlayRedHitSound(); //play the sound for hitting the red target
            explosion.Play(); //start the red explosion particle effect
            await Task.Delay(1000); //wait for 1 second
            explosion.Stop(); //stop the particle effect
        }

        if (gameObject.CompareTag("TargetBonus")) //when the bullet hits a blue target
        {
            ActivateTarget(gameObject, bonusBroken, 1); //change target's material and add 1 to score
            soundManager.PlayBlueHitSound(); //play the sound for hitting the blue target
            explosion.Play(); //start the blue explosion particle effect
            await Task.Delay(1000); //wait for 1 second
            explosion.Stop(); //stop the particle effect
        }

        if (gameObject.CompareTag("TargetPowerUp")) //when the bullet hits a green target
        {
            ActivateTarget(gameObject, powerUpBroken, 2); //change target's material and add 2 to score
            gameManager.powerUp = true; //trigger the power-up state
            soundManager.PlayGreenHitSound(); //play the sound for hitting the green target
            gameManager.powerUpShotsLeft += 2; //add 2 to the power up shots count
            explosion.Play(); //start the blue explosion particle effect
            await Task.Delay(1000); //wait for 1 second
            explosion.Stop(); //stop the particle effect
        }

        if (gameObject.CompareTag("TargetMultiply")) //when the bullet hits a pink target
        {
            ActivateTarget(gameObject, multiplyBroken, 2); //change target's material and add 2 to score
            playerController.DuplicateBullet(); //duplicate the bullet
            soundManager.PlayPinkHitSound(); //play the sound for hitting the pink target
            explosion.Play(); //start the blue explosion particle effect
            await Task.Delay(1000); //wait for 1 second
            explosion.Stop(); //stop the particle effect
        }
    }

    private void OnCollisionStay(Collision collision) //when the ball is stuck on a hit target
    {
        timer += Time.deltaTime; //increment the stuck timer duration

        if (timer > 1.5f) //when the stuck timer reaches 1.5 seconds
        {
            stuckTarget = gameManager.hitTargets[gameManager.hitTargets.Count - 1]; //reference the blocking target
            gameManager.hitTargets.Remove(gameManager.hitTargets[gameManager.hitTargets.Count - 1]); //remove the blocking target from the later removal sequence
            Destroy(stuckTarget); //destroy the blocking target
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        timer = 0; //reset the blocking timer if the bullet moves out of target's collider
    }

    void ActivateTarget(GameObject target, Material targetMaterialBroken, int scoreAdd)
    {
        gameManager.score += scoreAdd; //add the input value to the score value
        targetRenderer.material = targetMaterialBroken; //change the object's material to the input material
        gameManager.hitTargets.Add(target); //add the object to the list of hit targets
        target.tag = "Used"; //change object's tag to "Used"
        Destroy(GameObject.Find(target.name + "(Clone)")); //destroy a game object which contains the original object's name with "(Clone)" at the end /object's projection clone
        gameManager.scoreText.text = "" + gameManager.score; //change the value of the current score
    }

}
