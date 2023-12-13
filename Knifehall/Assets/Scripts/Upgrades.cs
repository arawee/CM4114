using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Upgrades : MonoBehaviour
{
    private Controller player;
    private FireHeal fire;

    // Functions for increasing object variables and subtracting levels
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Controller>();
        fire = GameObject.FindWithTag("Fire").GetComponent<FireHeal>();
    }

    public void LevelUpDamage()
    {
        if(player.level > 0)
        {
            player.damage += 1;
            LevelDown();
        }
    }

    public void LevelUpSpeed()
    {   
        if(player.level > 0)
        {
            player.speed += 1.0f;
            LevelDown();
        }
    }

    public void LevelUpReach()
    {
        if(player.level > 0)
        {
            fire.healDist += 1;
            LevelDown();
        }
    }

    public void LevelUpStrength()
    {
        if(player.level > 0)
        {   
            fire.healRate += 2.0f;
            LevelDown();
        }
    }

    public void LevelDown()
    {   
        fire.setPrefs();
        player.level -= 1;
    }
}