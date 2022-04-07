using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCatcherBehaviour : MonoBehaviour
{
    private int startDirection; //start direction in form of 0/1 numbers
    public bool moveLeft; //state of moving to the left
    public bool moveRight; //state of moving to the right
    public float speed = 8.0f; //catcher's movement speed
    private float xBound = 7f; //catcher's movement X axis bound value

    private void Start()
    {
        //set both move states to false
        moveLeft = false;
        moveRight = false;

        startDirection = Random.Range(0, 2); //getting random value to choose starting move direction from
        if (startDirection == 0) moveLeft = true; //if its 0 then start by moving left
        else if (startDirection == 1) moveRight = true; //if its 1 then start by moving right
    }
    private void Update()
    {
        if(moveLeft) //if catcher is moving left
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed); //move catcher object to the left by the chosen speed
        }
        else if(moveRight) //if catcher is moving right
        {
            transform.Translate(Vector3.right * Time.deltaTime * speed); //move catcher object to the right by the chosen speed
        }

        if(transform.position.x >= xBound) //when catcher reaches the right bound, move it to the left
        {
            moveRight = false;
            moveLeft = true;
        }
        else if(transform.position.x <= -xBound) //when catcher reaches the left bound, move it to the right
        {
            moveLeft = false;
            moveRight = true;
        }
    }
}
