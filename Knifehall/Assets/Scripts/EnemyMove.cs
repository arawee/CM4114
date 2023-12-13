using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{

    public AudioClip slimeWalk;
    public AudioClip slimeAttack;
    private AudioClip currentClip;

    Transform player;
    private float dist;

    private bool resetAttack = false;

    public float aggroDist = 15;
    public float speed = 0.5f;
    public float forceMagnitude = 10.0f;

    public Animator animator;
    private bool isFlipped;
    private bool fullAgro;

    public bool canMove = true;

    void Start()
    {
        // Set up variables
        animator.GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").transform;

    }

    void Update()
    {
        // Get distance to player
        dist = Vector3.Distance(player.position, transform.position);

        // If player is in range
        if ((dist <= aggroDist && canMove) || (fullAgro && canMove))
        {
            // Never stop following player
            fullAgro = true;

            // Get the direction from the enemy to the player
            Vector3 direction = (player.position - transform.position).normalized;
            
            // Flip sprite accordingly
            if((direction.x <= -0.71 || direction.z >= 0.71) && !isFlipped)
            {
                
                transform.localScale = new Vector3(1, 1, 1);
                isFlipped = true;

            } else if ((direction.x > -0.71 && direction.z < 0.71) && isFlipped)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                isFlipped = false;
            }

            // Move towards player
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            animator.SetBool("isMoving", true);

            // Play slimey sounds
            if (!GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().PlayOneShot(slimeWalk);    
            }

        } else
        {   
            // else stop
            animator.SetBool("isMoving", false);
        }

        // if close to player, play attack animation
        if(dist <= 3.5)
        {
            animator.SetTrigger("Attack");

            if (!resetAttack)
            {
                GetComponent<AudioSource>().PlayOneShot(slimeAttack);
                resetAttack = true;
            }

        } else 
        {
            resetAttack = false;
        }

        // and block movement if dead
        if(GetComponent<Health>().health <= 0 )
        {
            canMove = false;
        }
    }
}