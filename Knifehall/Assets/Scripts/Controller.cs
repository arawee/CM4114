using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{

    // MAIN PLAYER SCRIPT

    public KeyCode actionKey = KeyCode.Space;

    private AudioManager audioManager;
    public AudioClip calm;

    private CharacterController controller;
    public GameObject trace;

    public float speed = 5f;
    public int damage = 2;
    public float dashSpeed = 10f;
    public float dashDuration = 0.5f;
    private bool isDashing = false;
    public int xp = 0;
    public int level = 1;

    private float dashTimer = 0f;

    public float verticalStrength = 1.5f;

    private bool isFlipped = false;
    private bool isRunning = false;

    private Vector3 thisDirection;
    private Vector3 lastDirection = new Vector3(-1, 0, -1);

    public Animator animator;

    private GameObject attackArea = default;

    public Health health;
    private bool attacking = false;
    public float attackTime = 1f;
    private float timer = 0f;

    public AudioClip player_attack;
    public AudioClip player_dash;
    public AudioClip player_hurt;
    public AudioClip player_run;

    // Set up variables and assign action key (if it was remapped)
    void Start()
    {
        attackArea = transform.GetChild(1).gameObject;

        controller = GetComponent<CharacterController>();

        animator.GetComponent<Animator>();

        health = GetComponent<Health>();

        audioManager = FindObjectOfType<AudioManager>();
        if (PlayerPrefs.HasKey("ActionKey"))
        {
            actionKey = (KeyCode)PlayerPrefs.GetInt("ActionKey");
        }
    }

    void Update()
    {
        // Get movement input
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        move.z = Input.GetAxis("Vertical") * verticalStrength;

        // Rotate it to isometry
        move = Quaternion.Euler(0, 45f, 0) * move;

        // and normalise to get simple vector
        thisDirection = Vector3.Normalize(move);

        float horizontalInput = Input.GetAxis("Horizontal");

        if (move != Vector3.zero)
        {
            // If a player is moving, move and display animation

            animator.SetBool("isRunning", true);
            isRunning = true;

            if (move.x > 0 && move.z > 0)
            {
                lastDirection = new Vector3(1, 0, 1);
            } else if (move.x > 0 && move.z < 0) 
            {
                lastDirection = new Vector3(1, 0, -1);
                
            } else if (move.x < 0 && move.z > 0)
            {
                lastDirection = new Vector3(-1, 0, 1);

            } else if (move.x < 0 && move.z < 0)
            {
                lastDirection = new Vector3(-1, 0, -1);
            }

        } else {
            // if not runnning, stop
            animator.SetBool("isRunning", false);
            isRunning = false;
        }

        // Apply the dash if the dash key is pressed and the player is not already dashing
        if (Input.GetKeyDown(actionKey) && !isDashing)
        {
            audioManager.Play(player_dash);

            // Create a "trace" prefab at original dash position
            Instantiate(trace, transform.position, trace.transform.rotation);

            // and start dashing
            isDashing = true;
            animator.SetTrigger("dashTrigger");
            dashTimer = 0f;
        }

        // Block movement if attacking
        if(attacking)
        {
            controller.Move(new Vector3(0, 0, 0));
        }
        
        // if not dashing, move normally
        if (!isDashing && !attacking)
        {
            controller.Move(move * Time.deltaTime * speed);

        // else if dashing
        } else if (isDashing && !attacking) {

            // Update dash timer and check if the dash duration has passed
            dashTimer += Time.deltaTime;

            if (dashTimer >= dashDuration)
            {
                isDashing = false;
            }

            // and move in current direction
            if(isRunning) {

                controller.Move(thisDirection * Time.deltaTime * dashSpeed);

            } else {
                controller.Move(lastDirection * Time.deltaTime * dashSpeed);
            }
        }


        if (!attacking) {
            // Flip the player sprite when horizontal input is negative and is not locked in attack
            if (horizontalInput < 0 && !isFlipped)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                isFlipped = true;

            } else if (horizontalInput > 0 && isFlipped)
            {
                transform.localScale = new Vector3(1, 1, 1);
                isFlipped = false;
            }
        }

        // Level up if at 100xp
        if(xp >= 100)
        {
            level += 1;
            xp -= 100;
        }

        // Get input for attackinig
        Vector3 swing = new Vector3(Input.GetAxis("HorizontalKey"), 0, Input.GetAxis("VerticalKey"));
        swing = Quaternion.Euler(0, 45f, 0) * swing;

        // If attack input exists, and not already attacking
        if (swing != Vector3.zero && !attacking)
        {
            // Swing in specified direction
            if(isFlipped)
            {
                if (swing.x > 0 && swing.z > 0)
                {
                    Attack(135.0f);
                    Debug.Log("Swing down");

                } else if (swing.x > 0 && swing.z < 0) 
                {
                    Attack(45.0f);
                    Debug.Log("Swing right");

                } else if (swing.x < 0 && swing.z > 0)
                {
                    Attack(225.0f);
                    Debug.Log("Swing left");

                } else if (swing.x < 0 && swing.z < 0)
                {
                    Attack(-45.0f);
                    Debug.Log("Swing up");
                }
            } else 
            {
                if (swing.x > 0 && swing.z > 0)
                {
                    Attack(-45.0f);
                    Debug.Log("Swing down");

                } else if (swing.x > 0 && swing.z < 0) 
                {
                    Attack(225.0f);
                    Debug.Log("Swing right");

                } else if (swing.x < 0 && swing.z > 0)
                {
                    Attack(45.0f);
                    Debug.Log("Swing left");

                } else if (swing.x < 0 && swing.z < 0)
                {
                    Attack(135.0f);
                    Debug.Log("Swing up");
                }
            }
        }

        // Timer for time of attack
        if (attacking)
        {
            timer += Time.deltaTime;

            // Allow for another attack after it finised
            if (timer >= attackTime)
            {
                timer = 0;
                attacking = false;
                attackArea.SetActive(attacking);
            }
        }

        // I couldnt figure out why my player keeps flying off, so I locked it programatically
        if (GameObject.FindWithTag("Player").transform.position.y != 0.5f)
        {
            transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
        } 
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If a collision object is the enemy and its not dead, damage player for 10
        Transform childTransform = collision.gameObject.transform.GetChild(0);
        Animator enemyAnimator = childTransform.GetComponent<Animator>();

        if (enemyAnimator != null && !enemyAnimator.GetBool("Dead"))
        {
            audioManager.Play(player_hurt);
            health.Damage(10.0f);
        }
    }

    private void Attack(float degrees)
    {
        // Start attack loop and animation in specified direction
        attacking = true;

        audioManager.Play(player_attack);

        animator.SetTrigger("attackTrigger");

        attackArea.transform.rotation = Quaternion.Euler(0, degrees, 0);
        attackArea.SetActive(attacking);
    }
}