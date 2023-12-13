using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{

    // Pause when displayed (only at scene open)
    void Start()
    {
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Close on "E" press
        if (Input.GetKeyDown(KeyCode.E))
        {
            Time.timeScale = 1;
            gameObject.SetActive(false);
        }
    }
}