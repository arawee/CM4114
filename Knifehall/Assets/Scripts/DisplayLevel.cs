using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayLevel : MonoBehaviour
{
    private int level;
    public TMP_Text levelText;

    // Display current player level on screen
    void Start()
    {
        levelText.text = "Level: ";
    }

    void Update()
    { 
        level = GameObject.FindWithTag("Player").GetComponent<Controller>().level;
        levelText.text = "Level: " + level;
    }
}