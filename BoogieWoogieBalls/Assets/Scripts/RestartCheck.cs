using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartCheck : MonoBehaviour
{
    public bool isRestarted;
    void Awake()
    {
        DontDestroyOnLoad(gameObject); //dont destroy the game object when loading a new scene
    }

    private void Start()
    {
        isRestarted = false; //default state of "was the level restarted"
    }
}
