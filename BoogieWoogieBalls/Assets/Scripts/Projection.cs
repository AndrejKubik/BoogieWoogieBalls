using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Projection : MonoBehaviour
{
    private Scene simulationScene;
    private PhysicsScene projectionScene;
    [SerializeField]private Transform targetsParent;
    
    private GameObject ghostTarget;
    private BulletBehaviour ghostBullet;

    public GameObject walls;
    private GameObject wallsSimulation;

    public GameManager gameManager;

    private void Start() //before the game is loaded
    {
        CreateProjectionScene(); //run the method for creating a psysics sub-scene
    }

    void CreateProjectionScene()
    {
        simulationScene = SceneManager.CreateScene("Simulation", new CreateSceneParameters(LocalPhysicsMode.Physics3D)); //making a sub-scene with "Simulation" name
        projectionScene = simulationScene.GetPhysicsScene(); //storing the new scene into a variable and making it a physics scene

        foreach (Transform target in targetsParent) //for every single target object
        {
            ghostTarget = Instantiate(target.gameObject, target.position, target.rotation); //create a duplicate target
            ghostTarget.GetComponent<Renderer>().enabled = false; //disable it's visibility
            ghostTarget.tag = "GhostTarget"; //set it's tag to "GhostTarget"
            SceneManager.MoveGameObjectToScene(ghostTarget, simulationScene); //move it to the sub-scene
        }

        wallsSimulation = Instantiate(walls.gameObject, walls.transform.position, walls.transform.rotation); //create a clone of the level walls object
        SceneManager.MoveGameObjectToScene(wallsSimulation, simulationScene); //move it to the sub-scene
    }

    [SerializeField] private LineRenderer aimLine;
    [SerializeField] private int numberOfIterations; //value for normal state
    [SerializeField] private int pNumberOfIterations; //value for the power-up state
    public void SimulateTrajectory(BulletBehaviour bullet, Vector3 startPosition, float velocity)
    {
        ghostBullet = Instantiate(bullet, startPosition, transform.rotation); //create a duplicate bullet
        SceneManager.MoveGameObjectToScene(ghostBullet.gameObject, simulationScene); //move it to the sub-scene
        Rigidbody ghostRb = bullet.gameObject.GetComponent<Rigidbody>(); //copy the original's rigidbody settings
        ghostBullet.tag = "GhostBullet"; //set clone's tag to "GhostBullet"
        ghostBullet.LaunchBullet(ghostRb, velocity); //"launch" the clone bullet

        if (gameManager.powerUp) //if green power-up is hit
        {
            aimLine.positionCount = pNumberOfIterations; //set the number of iterations/aim length accordingly

            for (int i = 0; i < pNumberOfIterations; i++) //for every point of the aim line
            {
                projectionScene.Simulate(Time.fixedDeltaTime); //simulate the bullet path position at 1 frame in the future
                aimLine.SetPosition(i, ghostBullet.transform.position); //set the position of the aim line point at the position of the ghost bullet on frame number "i" in the future
            }
            Destroy(ghostBullet.gameObject); //destroy the bullet clone right after the aim line is finished
        }
        else //pretty much the same with different value for the aim line length
        {
            aimLine.positionCount = numberOfIterations;
            for (int i = 0; i < numberOfIterations; i++)
            {
                projectionScene.Simulate(Time.fixedDeltaTime);
                aimLine.SetPosition(i, ghostBullet.transform.position);
            }
            Destroy(ghostBullet.gameObject);
        }
    }
}
