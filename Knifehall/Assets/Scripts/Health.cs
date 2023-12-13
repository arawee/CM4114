using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    private AudioManager audioManager;

    public float health = 100f;
    public int xpValue = 3;
    
    public float maxHealth = 100f;
    private bool dead = false;

    public Animator animator;

    Transform player;

    public EnemyMove enemymove;

    public GameObject drop;
    public GameObject deathScreen;
    public GameObject xpDrop;
    public float dropChance = 0.25f;

    void Start()
    {
        animator.GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").transform;
    }

    // The "Jay" key setup {GOD DAMAGE}
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Damage(1.0f);
        }
    }

    public void Damage(float amount) 
    {
        // Any damaged object, decrease health
        animator.SetTrigger("Hurt");

        if (health - amount > maxHealth)
        {
            this.health = maxHealth;
        } else {
            this.health -= amount;
        }

        // Kill if on 0 HP
        if (health <= 0)
        {
            if(dead == false)
            {
                dead = true;
                Die();
            }
        }

        // Stun enemy if it was damaged
        if(GetComponent<EnemyMove>() != null && health > 0)
        {
            StopMovementForDuration(1f);
        }
    }

    // Function to stop movement for a specified duration
    public void StopMovementForDuration(float duration)
    {
        StartCoroutine(StopMovementCoroutine(duration));
    }

    // Coroutine to stop movement for a specified duration
    private IEnumerator StopMovementCoroutine(float duration)
    {
        GetComponent<EnemyMove>().canMove = false;

        yield return new WaitForSeconds(duration);

        GetComponent<EnemyMove>().canMove = true;
    }

    // Function for increasing HP
    public void IncreaseHealth(float amountPerSecond)
    {
        health += amountPerSecond;
    }

    private void Die()
    {

        // If a slime
        if(GetComponent<EnemyMove>() != null)
        {
            // Block movement and collisions
            GetComponent<EnemyMove>().canMove = false;
            GetComponent<BoxCollider>().enabled = false;

            Rigidbody rb = GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezePosition;
            
            // Random chance for dropping HP potions
            float randomValue = Random.Range(0f, 1f);

            if(randomValue <= dropChance)
            {
                // If RNG good, drop potion
                Instantiate(drop, transform.position, drop.transform.rotation);
            }

            // Create new xp object for each XP value
            while(xpValue > 0)
            {
                Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f), 0.14f, Random.Range(-1f, 1f));
                Instantiate(xpDrop, transform.position + randomOffset, drop.transform.rotation);
                xpValue -= 1;
            }
        }

        // If player was killed, pause the game
        if(GetComponent<Controller>() != null)
        {
            Time.timeScale = 0;
            deathScreen.SetActive(true);
        }

        animator.SetBool("Dead", true);
    }
}