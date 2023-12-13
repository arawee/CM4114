using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    private const float maxHealth = 100f;
    public float health = maxHealth;
    private Image healthbar;

    // Display health on screen
    void Start()
    {
        healthbar = GetComponent<Image>();
    }

    void Update()
    {
        health = GameObject.FindWithTag("Player").GetComponent<Health>().health;
        healthbar.fillAmount = health / maxHealth;
    }
}