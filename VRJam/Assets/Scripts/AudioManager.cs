using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance; // Singleton instance

    private AudioSource audioSource;

    // Public AudioClips that can be assigned directly in the Inspector
    public AudioClip walkoutmusic;
    public AudioClip walkoutsound;
    public AudioClip Right;
    public AudioClip Wrong;
    public AudioClip BackgroundMusic;
    public AudioClip BackgroundNoise;

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of the AudioManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep AudioManager between scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate AudioManager instances
        }
    }

    private void Start()
    {
        // Set up an AudioSource on the AudioManager (can be on the same GameObject)
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("No AudioSource found on AudioManager. Please attach one.");
        }
    }

    // Methods to play each specific sound clip
    public void PlayWalkoutmusic()
    {
        audioSource.loop = false;
        PlayClip(walkoutmusic);
    }

    public void PlayWalkoutSound()
    {
        audioSource.loop = false;
        PlayClip(walkoutsound);
    }

    public void PlayRight()
    {
        audioSource.loop = false;
        PlayClip(Right);
    }

    public void PlayWrong()
    {
        audioSource.loop = false;
        PlayClip(Wrong);
    }

    public void PlayBackgroundMusic()
    {
        audioSource.loop = true;
        PlayClip(BackgroundMusic);
    }

    public void PlayBackgroundNoise()
    {
        audioSource.loop = true;
        PlayClip(BackgroundNoise);
    }

    // Private helper method to play a given AudioClip
    private void PlayClip(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("AudioClip or AudioSource is missing.");
        }
    }
}
