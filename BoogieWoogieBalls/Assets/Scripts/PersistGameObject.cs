using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersistGameObject : MonoBehaviour
{
    public Slider loadingSlider;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject); //dont destroy the game object when loading a new level
    }

    private void Update()
    {
        if(loadingSlider.value == 1.0f) //when loading bar reaches 100% 
        {
            Destroy(gameObject); //destroy the loading bar
        }
    }
}
