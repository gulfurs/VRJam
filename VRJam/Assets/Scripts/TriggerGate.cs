using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerGate : MonoBehaviour
{   
    public GameObject gate;

    private Animator gateAnimator;
    private AudioManager audioManager;

    public AudienceMovement entry;

    private void Start()
    {
        gateAnimator = gate.GetComponent<Animator>();
        audioManager = AudioManager.instance;

        if (audioManager == null){Debug.LogError("AudioManager instance not found.");}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (gateAnimator != null)
            {
                gateAnimator.SetTrigger("gateTrigger");
                audioManager.PlayWalkoutMusic();
                audioManager.PlayWalkoutSound();
                entry.Entry();
            }
        }
    }
}
