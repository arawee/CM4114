using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class FireHeal : MonoBehaviour
{

    GameObject player;
    public GameObject upgradeCanvas;
    public GameObject loadingScreen;
    public GameObject winCanvas;
    public GameObject dashHint;
    public GameObject cylinder;
    Transform cylinderTransform;
    Transform playerTransform;
    Vector3 scaleFire;

    public GameObject settings;
    public AudioMixer audioMixer;
    private bool epressed = false;

    public GameObject hint;
    public GameObject hintEnd;
    public GameObject hintPauseBig;
    public GameObject hintPauseSmall;
    public bool cleared = false;
    private bool menuOpen = false;
    private float dist;
    string currentScene;
    public float healDist = 5;
    public float healRate = 5f;

    Controller playerStats;

    Health playerHealth;

    private float timer;
    private bool isImageDisplayed = false;

    void Start()
    {   
        // Set up varioables
        player = GameObject.FindWithTag("Player");
        playerStats = player.GetComponent<Controller>();
        playerTransform = GameObject.FindWithTag("Player").transform;
        playerHealth = player.GetComponent<Health>();

        cylinderTransform = cylinder.transform;
        scaleFire = cylinderTransform.localScale;

        // Scale the fire object
        scaleFire.x = healDist * 1.5f;
        scaleFire.z = healDist * 1.5f;

        cylinderTransform.localScale = scaleFire;

        // If opening a new game, reset preferences
        currentScene = SceneManager.GetActiveScene().name;

        if(currentScene == "Game")
        {
            PlayerPrefs.SetFloat("Health", playerHealth.health);
            PlayerPrefs.SetInt("Xp", playerStats.xp);
            PlayerPrefs.SetInt("Level", playerStats.level);
            PlayerPrefs.SetInt("Damage", playerStats.damage);
            PlayerPrefs.SetFloat("Speed", playerStats.speed);
            PlayerPrefs.SetFloat("FireReach", healDist);
            PlayerPrefs.SetFloat("FireStrength", healRate);
        } else
        {
            epressed = true;    
        }
    }

    // Open settings and pause
    public void OpenSettings()
    {

        if(upgradeCanvas != null)
        {
            if(upgradeCanvas.activeSelf)
            {
                upgradeCanvas.SetActive(false);
            }
        }
        
        settings.SetActive(true);
        Time.timeScale = 0;
    }

    // Close settings canvas and reset time
    public void CloseSettings()
    {
        if(currentScene != "MainMenu")
        {
            playerStats.actionKey = (KeyCode)PlayerPrefs.GetInt("ActionKey");
        }

        settings.SetActive(false);
        Time.timeScale = 1;
    }

    // Set volume in audio mixer
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }

    // Update is called once per frame
    void Update()
    {
        // If space was pressed, reset hint timer
        if (Input.GetKeyDown(KeyCode.Space))
        {
            timer = 0;
            HideImage();
        }
        else
        {
            // else, if 10s without pressing Dash, display hint
            timer += Time.deltaTime;

            if (timer >= 10  && !isImageDisplayed)
            {
               ShowImage();
            }
        }

        // Manage hint displays
        dist = Vector3.Distance(playerTransform.position, transform.position);
        
        // When in fire distance
        if (dist <= healDist)
        {
            // If player accessed the upgrades and is in range, display small hint
            if(epressed == true && !cleared)
            {
                hintPauseSmall.SetActive(true);
            }

            // If E pressed in range
            if (Input.GetKeyDown(KeyCode.E))
            {
                // Hide "First time" large hint
                if(epressed == false)
                {
                    hintPauseBig.SetActive(false);
                    epressed = true;
                }

                // Update the size of the fire
                scaleFire.x = healDist * 1.5f;
                scaleFire.z = healDist * 1.5f;

                cylinderTransform.localScale = scaleFire;

                // Save prefs
                setPrefs();

                if(menuOpen)
                {
                    if(cleared)
                    {
                        // If E pressed on an open screes, press again to progress to next screen
                        if (currentScene == "Game")
                        {
                            StartCoroutine(LoadOffScene(2));
                        } else if (currentScene == "Level2")
                        {
                            StartCoroutine(LoadOffScene(3));
                        } else if (currentScene == "Level3")
                        {
                            winCanvas.SetActive(true);
                        }
                        
                        Debug.Log("Xp: " + PlayerPrefs.GetInt("Xp", 0));
                        Debug.Log("Level: " + PlayerPrefs.GetInt("Level", 0));
                        Debug.Log("Damage: " + PlayerPrefs.GetInt("Damage", 0));
                        Debug.Log("Speed: " + PlayerPrefs.GetFloat("Speed", 0));
                        Debug.Log("FireReach: " + PlayerPrefs.GetFloat("FireReach", 0));
                        Debug.Log("FireStrength: " + PlayerPrefs.GetFloat("FireStrength", 0));
                    }

                    // Hide menu and play game
                    upgradeCanvas.SetActive(false);
                    menuOpen = false;

                    Time.timeScale = 1;
                } else 
                {
                    // If E pressed and menu is closed, open it and pause
                    Time.timeScale = 0;
                    upgradeCanvas.SetActive(true);
                    menuOpen = true;
                }
            }

            // Keep increasign HP while in range
            playerHealth.IncreaseHealth(healRate * Time.deltaTime);
        }
        
        // Hide small hint when not desirable
        if(dist > healDist || cleared)
        {
            hintPauseSmall.SetActive(false);
        }

        // Display correct hints and allow progress to next level
        if(cleared)
        {
            epressed = true;
            hintPauseBig.SetActive(false);
            hint.SetActive(true);
            hintEnd.SetActive(true);
        }
    }

    // Show dash hint
    void ShowImage()
    {
        dashHint.gameObject.SetActive(true);
        isImageDisplayed = true;
    }

    // Hide dash hint
    void HideImage()
    {
        dashHint.gameObject.SetActive(false);
        isImageDisplayed = false;
    }

    // Enum. function for async loading screen
    public IEnumerator LoadOffScene(int scene)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene);

        loadingScreen.SetActive(true);

        while(!operation.isDone)
        {
            yield return null;
        }
    }
    
    // Function for saving prefs
    public void setPrefs()
    {
        PlayerPrefs.SetFloat("Health", playerHealth.health);
        PlayerPrefs.SetInt("Xp", playerStats.xp);
        PlayerPrefs.SetInt("Level", playerStats.level);
        PlayerPrefs.SetInt("Damage", playerStats.damage);
        PlayerPrefs.SetFloat("Speed", playerStats.speed);
        PlayerPrefs.SetFloat("FireReach", healDist);
        PlayerPrefs.SetFloat("FireStrength", healRate);

        PlayerPrefs.Save();
    }
}