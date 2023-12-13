using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadPlayer : MonoBehaviour
{

    FireHeal fire;
    Controller controller;

    private int xp;
    private int level;
    private float speed;
    private int damage;
    private float healDist;
    private float healRate;

    // Get player variables from player prefs and apply
    void Start()
    {
        controller = gameObject.GetComponent<Controller>();
        fire = GameObject.FindWithTag("Fire").GetComponent<FireHeal>();

        controller.xp = PlayerPrefs.GetInt("Xp");
        controller.level = PlayerPrefs.GetInt("Level");
        controller.damage = PlayerPrefs.GetInt("Damage");
        controller.speed = PlayerPrefs.GetFloat("Speed");
        fire.healDist = PlayerPrefs.GetFloat("FireReach");
        fire.healRate = PlayerPrefs.GetFloat("FireStrength");
    }
}