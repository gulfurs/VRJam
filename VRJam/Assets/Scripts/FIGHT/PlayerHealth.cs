using System.Collections;
using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHP = 200f;              // Maximum health points
    private float currentHP;                // Current health points

    [Header("3D Health Bar")]
    public TextMeshPro healthTextMesh;         // Reference to a TextMesh in the scene

    public float swordDamage = 20f;

    void Start()
    {
        currentHP = maxHP;
        UpdateHealthText();                 // Initialize the health display
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Assuming the enemy has the Enemy component
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(swordDamage);  // Call the TakeDamage method on the enemy
            }
        }
    }

    public void TakeDmg(float dmg)
    {
        currentHP = Mathf.Max(0, currentHP - dmg);  // Reduce HP without going below 0
        UpdateHealthText();

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void UpdateHealthText()
    {
        int numBars = Mathf.CeilToInt(currentHP / 10f);  // Calculate number of '-' to show
        healthTextMesh.text = new string('-', numBars);  // Update the TextMesh with health bars
    }

    void Die()
    {
        Debug.Log("YOU DEAD SON");
        // Add any death logic here (e.g., respawn or game over)
    }
}
