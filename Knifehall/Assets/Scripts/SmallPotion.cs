using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallPotion : MonoBehaviour
{

    Transform player;
    
    Health playerHealth;

    private AudioManager audioManager;
    public AudioClip heal;

    public float magnetDist = 15;
    public float pickupDist = 2.5f;
    public float deleteDist = 1;
    public float speed = 1f;
    public float forceMagnitude = 10.0f;
    public float healAmount = 15.0f;

    private float dist;

    void Start()
    {
        // Set up variables
        player = GameObject.FindWithTag("Player").transform;
        playerHealth = player.GetComponent<Health>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    void Update()
    {
        // Same as xp, move towards player and add xp/destroy itself
        dist = Vector3.Distance(player.position, transform.position);

        if (dist <= magnetDist)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

            if (dist <= pickupDist)
            {
                speed = 3f;
            }

            if (dist <= deleteDist)
            {
                audioManager.PlayPickup(heal);
                playerHealth.IncreaseHealth(healAmount);
                Destroy(gameObject);
            }
        }
    }
}