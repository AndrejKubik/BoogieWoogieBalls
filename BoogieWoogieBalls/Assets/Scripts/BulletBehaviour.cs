using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public GameObject bullet;
    public Rigidbody bulletRb;
    public float shotForce = 20.0f; //strength of a shot
    private float destroyBound = -30.0f; //Y bound where bullets should be destroyed

    private GameManager gameManager;
    private SoundManager soundManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); //setting up GameManager script reference
        soundManager = FindObjectOfType<SoundManager>(); //setting up SoundManager script reference
    }
    private void OnEnable() //when the bullet spawns
    {
        bulletRb = GetComponent<Rigidbody>(); //setting spawned bullet's rigidbody component
        LaunchBullet(bulletRb, -shotForce); //launching the bullet towards a chosen direction with the chosen velocity
    }

    public void LaunchBullet(Rigidbody objectRb, float launchForce)
    {
        objectRb.velocity = transform.up * launchForce; //setting the object's velocity to the chosen value in the upwards direction
    }

    private void Update()
    {
        if(gameObject.transform.position.y < destroyBound) //when the bullet falls bellow the bottom Y-bound
        {
            Destroy(gameObject); //destroy the bullet
            gameManager.bulletsOnScreen -= 1;

            if (gameManager.allBulletsDestroyed && gameManager.bulletsOnScreen == 0) //when all the bullets on screen have fallen down
            {
                gameManager.allBulletsDestroyed = false; //reset the trigger for them all being destroyed
                gameManager.destroyTargets = true; //activate the trigger for the hit target removal sequence
            }

            if (gameManager.ammo == 0) //when player fails to hit the catcher while at 0 ammo
            {
                gameManager.ammo--; //reduce ammo count value by 1

                //(game is over if ammo < 0) thats the reason for going bellow 0
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Catcher")) //when the bullet falls into the catcher
        {
            Destroy(gameObject); //destroy the bullet
            gameManager.bulletsOnScreen--; //reduce the bullets on screen count by 1
            gameManager.ammo++; //return 1 ammo to the player
            gameManager.bulletsLeft.text = "" + gameManager.ammo; //change the value of bullets left on screen
            soundManager.PlayCatcherSound(); //play the sound for catching a bullet
            if (gameManager.allBulletsDestroyed && gameManager.bulletsOnScreen == 0) //when all the bullets on screen have fallen down
            {
                gameManager.allBulletsDestroyed = false; //reset the trigger for them all being destroyed
                gameManager.destroyTargets = true; //activate the trigger for the hit target removal sequence
            }
        }
    }

    
}
