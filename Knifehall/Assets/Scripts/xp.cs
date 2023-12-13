using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class xp : MonoBehaviour
{

    Transform player;
    private int playerXp;
    
    private AudioManager audioManager;
    public AudioClip heal;

    public float magnetDist = 15;
    public float pickupDist = 2.5f;
    public float deleteDist = 1;
    public float speed = 2f;
    public float forceMagnitude = 10.0f;
    public int xpAmount = 50;
    
    private float dist;

    void Start()
    {
        // Set up variables
        player = GameObject.FindWithTag("Player").transform;
        audioManager = FindObjectOfType<AudioManager>();
    }

    void Update()
    {
        // Same as enemy, get dfistance to player, move towards, and destroy itself when close.
        dist = Vector3.Distance(player.position, transform.position);

        if (dist <= magnetDist)
        {

            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

            if (dist <= pickupDist)
            {
                audioManager.PlayPickup(heal);
                speed = 5f;
            }

            if (dist <= deleteDist)
            {
                // Add XP to player
                player.GetComponent<Controller>().xp += xpAmount;
                Destroy(gameObject);
            }
        }
    }
}