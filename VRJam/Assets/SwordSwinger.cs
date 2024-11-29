using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSwinger : MonoBehaviour
{
    public float swordDamage = 20f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("CRASH");
            Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
            {
            enemy.TakeDamage(swordDamage);  // Call the TakeDamage method on the enemy
            }
        }
    }

}
