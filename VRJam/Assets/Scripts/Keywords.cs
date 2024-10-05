using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Keywords : MonoBehaviour
{

    // Dictionary to store keyword-prefab pairs
    private Dictionary<string, GameObject> keywordPrefabMap = new Dictionary<string, GameObject>();

    // Reference to spawn point
    public Transform spawnPoint;

    void Start()
    {
        // Add keyword-prefab mappings here
        keywordPrefabMap.Add("jupiter", Resources.Load<GameObject>("Prefabs/JupiterPrefab"));  // Ensure the prefab is in a "Resources/Prefabs" folder
        // Add more keywords as needed
    }

    public void HandleTranscription(string transcription)
    {

        // Check for any keywords
        foreach (var keyword in keywordPrefabMap.Keys)
        {
            if (transcription.ToLower().Contains(keyword))
            {
                SpawnPrefab(keywordPrefabMap[keyword]);
            }
        }
    }

    void SpawnPrefab(GameObject prefab)
    {
        if (prefab != null && spawnPoint != null)
        {
            Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
            Debug.Log("Spawned " + prefab.name);
        }
        else
        {
            Debug.LogWarning("Prefab or spawn point is not assigned.");
        }
    }
}
