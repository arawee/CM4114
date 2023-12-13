using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XpBar : MonoBehaviour
{

    private const float maxXp = 100f;

    public float xp = 0;

    private Image xpbar;

    // Display XP on screen
    void Start()
    {
        xpbar = GetComponent<Image>();
    }

    void Update()
    {
        xp = GameObject.FindWithTag("Player").GetComponent<Controller>().xp;
        
        xpbar.fillAmount = xp / maxXp;
    }
}