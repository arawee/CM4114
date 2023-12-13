using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource EffectsSource;
    public AudioSource MusicSource;
    public AudioSource PickupSource;

    public AudioClip menu;
    public AudioClip calm;
    public AudioClip campfire;
    public AudioClip fight;

    private bool cleared;
    private bool calmBool = true;

    public GameObject fire;

    // Play fight music at game opening

    void Start()
    {
        PlayMusic(fight);
    }

    // Actively check for enemies to swich soundtrack
    void Update()
    {
        CheckForEnemies();
    }
    
    // Funcions for sound separated into channels
    public void Play(AudioClip clip)
    {
        EffectsSource.clip = clip;
        EffectsSource.Play();
    }
    
    public void PlayPickup(AudioClip clip)
    {
        EffectsSource.clip = clip;
        EffectsSource.Play();
    }

    public void PlayMusic(AudioClip clip)
    {
        MusicSource.clip = clip;
        MusicSource.loop = true; // Set the loop property to true
        MusicSource.Play();
    }

    // Check if there are any enemies with positive health
    // if not, change music and pass variables on
    void CheckForEnemies()
    {
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        List<GameObject> liveEnemies = new List<GameObject>();

        foreach (GameObject enemy in allEnemies)
        {
            Health healthScript = enemy.GetComponent<Health>();

            if (healthScript != null && healthScript.health > 0)
            {
                // The enemy has health greater than 0
                Debug.Log("Enemy has health greater than 0");
                liveEnemies.Add(enemy);
            }
        }

        if (liveEnemies.Count > 0)
        {
            //Debug.Log("Enemies found");
        }
        else
        {
            cleared = true;
            //Debug.Log("No enemies found");
        }

        if (cleared == true && calmBool == true)
        {
            PlayMusic(calm);

            fire.GetComponent<FireHeal>().cleared = true;

            calmBool = false;
        }
    }
}