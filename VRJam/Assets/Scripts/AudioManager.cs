using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource musicSource;  
    public AudioSource sfxSource;    

    public AudioClip walkoutmusic;
    public AudioClip walkoutsound;
    public AudioClip Right;
    public AudioClip Wrong;
    //public AudioClip BackgroundMusic;
    public AudioClip BackgroundNoise;
    public AudioClip audienceNoise; 

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (musicSource == null || sfxSource == null)
        {
            Debug.LogError("No AudioSources found on AudioManager. Please attach both musicSource and sfxSource.");
        }
    }

    public void PlayWalkoutMusic()
    {
        if (musicSource != null)
        {
            musicSource.loop = false;
            PlayClip(musicSource, walkoutmusic);
        }
    }

    public void PlayWalkoutSound()
    {
        if (sfxSource != null)
        {
            sfxSource.loop = false;
            PlayClip(sfxSource, walkoutsound);
        }
    }

    private IEnumerator PlayAudienceNoiseAfterWalkout()
    {
        yield return new WaitForSeconds(walkoutsound.length);

        PlayAudienceNoise();
    }

    private void PlayAudienceNoise()
    {
        if (sfxSource != null)
        {
            //sfxSource.loop = false;
            PlayClip(sfxSource, audienceNoise);
        }
    }

    public void PlayRight()
    {
        if (sfxSource != null)
        {
            sfxSource.loop = false;
            PlayClip(sfxSource, Right);
        }
    }

    public void PlayWrong()
    {
        if (sfxSource != null)
        {
            sfxSource.loop = false;
            PlayClip(sfxSource, Wrong);
        }
    }

   /* public void PlayBackgroundMusic()
    {
        if (musicSource != null)
        {
            musicSource.loop = true;
            PlayClip(musicSource, BackgroundMusic);
        }
    }*/

    public void PlayBackgroundNoise()
    {
        if (musicSource != null)
        {
            musicSource.loop = true;
            PlayClip(musicSource, BackgroundNoise);
        }
    }

    private void PlayClip(AudioSource source, AudioClip clip)
    {
        if (source != null && clip != null)
        {
            source.clip = clip;
            source.Play();
        }
        else
        {
            Debug.LogWarning("AudioClip or AudioSource is missing.");
        }
    }
}
