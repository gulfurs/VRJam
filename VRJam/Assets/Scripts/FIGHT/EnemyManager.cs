using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyManager : MonoBehaviour
{
    public GameObject savedEnemy; // Prefab of the enemy to spawn
    public float spawnRadius = 59f; // Radius of the circle arc
    public float arcAngle = 180f; // Angle of the arc in degrees
    public Transform spawnCenter; // Center point of the arc
    [SerializeField]
    private int index = 1; // Number of enemies to spawn, starts at 1

    public event Action noEnemyEvent;
    public GameObject playerWeapon;
    private List<GameObject> activeEnemies = new List<GameObject>();

    void Start()
    {
        savedEnemy = transform.GetChild(0).gameObject;

        //Destroy(savedEnemy);

        var getSentenceCreation = FindFirstObjectByType<SentenceCreation>();
        getSentenceCreation.loseEvent += losingEvent;
    }

    void losingEvent()
    {
        playerWeapon.SetActive(true);
        // SPAWN MULTIPLE ENEMIES RANDOMLY BASED ON INDEX
        for (int i = 0; i < index; i++)
        {
            SummonEnemyInArc();
        }

        index++; // INCREASE INCREASE INCREASE INCREASE INCREASE INCREASE INCREASE INCREASE INCREASE INCREASE INCREASE INCREASE INCREASE
    }

    void SummonEnemyInArc()
    {
        // CIRCLE ANGLE
        float randomAngle = UnityEngine.Random.Range(-arcAngle / 2, arcAngle / 2);

        // RADIANS
        float radians = randomAngle * Mathf.Deg2Rad;

        // RADIANS CALLED FROM CENTER
        Vector3 spawnPosition = spawnCenter.position + new Vector3(
            Mathf.Cos(radians) * spawnRadius,
            0,
            Mathf.Sin(radians) * spawnRadius
        );

        // INSTANTIATE ENEMY
        GameObject newEnemy = Instantiate(savedEnemy, spawnPosition, Quaternion.identity);
        newEnemy.transform.SetParent(transform, true); 
        activeEnemies.Add(newEnemy);
    }

    void HandleEnemyDestroyed(GameObject enemy)
    {
        // REMOVE ENEMY FROM LIST
        activeEnemies.Remove(enemy);

        // IF ENEMIES EQUAL 0?
        if (activeEnemies.Count == 0)
        {
            noEnemyEvent?.Invoke();
            playerWeapon.SetActive(false);
        }
    }
}
