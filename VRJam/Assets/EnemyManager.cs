using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyManager : MonoBehaviour
{
    private GameObject savedEnemy;
    // Start is called before the first frame update
    void Start()
    {
        savedChild = transform.GetChild(0).gameObject;

        Destroy(savedEnemy);
        SentenceCreation.loseEvent += losingEvent;
    }

    void losingEvent() {
        //Instantiate.
        //index++;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
