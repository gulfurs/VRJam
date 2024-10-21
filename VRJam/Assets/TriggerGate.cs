using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerGate : MonoBehaviour
{   
    public GameObject gate;

    private Animator gateAnimator;

    private void Start()
    {
        gateAnimator = gate.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (gateAnimator != null)
            {
                gateAnimator.SetTrigger("gateTrigger");
            }
        }
    }
}
