using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHP = 200f;
    public Slider HPbar;
    private float currentHP;

    void Start(){
        currentHP = maxHP;
        UpdateHPbar();
        
    }

    public void TakeDmg(float dmg){
        currentHP = Mathf.Max(0, currentHP - dmg);
        UpdateHPbar();

        if (currentHP <= 0){
            Die();
        }
    }

    void UpdateHPbar(){
        if (HPbar != null){
            HPbar.value = currentHP / maxHP;
        }
    }
    
    void Die(){
        Debug.Log("YOU DEAD SON");
    }

}
