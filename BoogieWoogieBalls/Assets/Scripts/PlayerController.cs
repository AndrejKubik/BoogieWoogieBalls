using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class PlayerController : MonoBehaviour
{
    public GameManager gameManager;
    public SoundManager soundManager;
    public BulletBehaviour bulletBehaviour;
    public Projection projection;

    public GameObject player;
    public Rigidbody playerRb;

    public LineRenderer aimLineRenderer;

    //input and player positions
    private Vector3 touchPos;
    private Vector3 playerPos;

    //bullet spawn and the bullet variables
    public GameObject bulletSpawn;
    public GameObject bullet;
    public GameObject bullet2;
    public Vector3 bulletPosition;
    public Rigidbody bullet2Rb;

    //player turning angle variables
    private float turnAngleX;
    private float turnAngleY;
    private float turnAngle;
    private float turnAngleFinal;

    public bool canFire; //fire-ready state
    public bool canAim; //aim-ready state

    private void Start()
    {
        canFire = true; //disable shooting
        canAim = true; //disable aiming
    }
    private void Update()
    {
        if(Input.GetMouseButton(0) && gameManager.gameActive && !gameManager.gamePaused) //while mouse button is held if the game is active and unpaused
        {
            playerPos = Camera.main.WorldToScreenPoint(player.transform.position); //getting player position relative to the game camera
            touchPos = Input.mousePosition; //getting click position

            //calculating turn angle coordinates
            turnAngleX = touchPos.x - playerPos.x;
            turnAngleY = touchPos.y - playerPos.y;

            turnAngle = Mathf.Atan2(turnAngleY, turnAngleX) * Mathf.Rad2Deg; //calculating the turn angle
            turnAngleFinal = turnAngle + 90f; // "+ 90f" to correct the orientation

            if (turnAngleFinal > 60f || turnAngleFinal < -60f || !canAim) //if player is aiming outside of aim bounds or aiming isn't allowed
            {
                playerRb.constraints = RigidbodyConstraints.FreezeRotationZ; //lock player object's rotation to a appropriate aim bound rotation or totally
            }
            else if (turnAngleFinal <= 60f || turnAngleFinal >= -60f) //while player is aiming within the aim bounds
            {
                if(canAim) //if aiming is allowed
                {
                    player.GetComponent<LineRenderer>().enabled = true; //make the aim line visible
                    playerRb.constraints = RigidbodyConstraints.None; //unlock the rotation of the player object
                    player.transform.rotation = Quaternion.Euler(0, 0, turnAngleFinal); //turn the player object towards the touch/click position
                    projection.SimulateTrajectory(bulletBehaviour, bulletSpawn.transform.position, -bulletBehaviour.shotForce); //build the aim line by simulating the ghost bullet path
                }
            } 
        }
    }

    public void FireSequence()
    {
        if(canFire && gameManager.gameActive)
        {
            Instantiate(bullet, bulletSpawn.transform.position, player.transform.rotation); //spawn a bullet at the according spawn position
            canFire = false; //disable further shooting
            canAim = false; //freeze the player object rotation
            gameManager.ammo--; //reduce player ammo by 1
            gameManager.bulletsLeft.text = "" + gameManager.ammo; //change the value of bullets left on screen
            soundManager.PlayFireSound(); //play the firing sound effect
            player.GetComponent<LineRenderer>().enabled = false; //turn off the aim line
            gameManager.bulletsOnScreen += 1; //increase the number of active bullets
            if (gameManager.powerUp) gameManager.powerUpShotsLeft -= 1; //if the power up is active consume one shot
        }
        
    }

    public async void DuplicateBullet()
    {
        await Task.Delay(50); //wait 
        bulletPosition = GameObject.FindGameObjectWithTag("Bullet").transform.position; //search for main bullet's current position
        bullet2 = Instantiate(bullet, bulletPosition, transform.rotation); //spawn a duplicate bullet at main bullet's position
        bullet2Rb = bullet2.GetComponent<Rigidbody>(); //get a reference for second bullet's rigidbody
        gameManager.bulletsOnScreen += 1; //increase the number of active bullets
        bullet2Rb.velocity = transform.up * bulletBehaviour.shotForce; //push the duplicate bullet up by the set shot force
    }
}

