using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{

    public GameObject fire;

    // Functions for loading game and quitting
    public void StartGame()
    {
        StartCoroutine(fire.GetComponent<FireHeal>().LoadOffScene(1));
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Closed the game.");
    }
}