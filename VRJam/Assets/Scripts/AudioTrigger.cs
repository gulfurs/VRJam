using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
 public GameObject audioSourceLocation; 
    private AudioSource audioSource;

    private void Start()
    {
        
        if (audioSourceLocation != null)
        {
            audioSource = audioSourceLocation.GetComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}
