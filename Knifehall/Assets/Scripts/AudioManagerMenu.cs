using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerMenu : MonoBehaviour
{
    public AudioSource EffectsSource;
    public AudioSource MusicSource;

    public AudioClip menu;

    // Play menu music at initial opening
    void Start()
    {
        PlayMusic(menu);
    }

    // Channels, similar to main audio manager

    public void Play(AudioClip clip)
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
}