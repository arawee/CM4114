using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMain : MonoBehaviour
{
    public GameObject fire;
    
    // Functions for restarting and going to main menu
    public void Respawn()
    {
        StartCoroutine(fire.GetComponent<FireHeal>().LoadOffScene(1));
    }

    public void Restart()
    {        
        StartCoroutine(fire.GetComponent<FireHeal>().LoadOffScene(0));
    }
}