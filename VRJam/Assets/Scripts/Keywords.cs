using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Keywords : MonoBehaviour
{
// Dictionary to store keyword-prefab pairs
    public Dictionary<string, GameObject> keywordPrefabMap = new Dictionary<string, GameObject>();

    // Array of keyword-prefab pairs for Unity Inspector
    [System.Serializable]
    public class KeywordPrefabPair
    {
        public string keyword;        // The keyword to recognize
        public GameObject prefab;     // The prefab associated with the keyword
    }

    public KeywordPrefabPair[] keywordPrefabPairs; // Expose in Inspector

    // Reference to spawn point
    public Transform spawnPoint;

    void Start()
    {
        // Populate the dictionary from the array
        keywordPrefabMap = new Dictionary<string, GameObject>();
        foreach (var pair in keywordPrefabPairs)
        {
            keywordPrefabMap.Add(pair.keyword.ToLower(), pair.prefab); // Case insensitive key
        }
    }

    public void HandleTranscription(string transcription)
    {

        //Debug.Log("Transcription in Keywords: " + transcription);
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
            Debug.Log("Spawned " + prefab.name + " at " + spawnPoint.position);
        }
        else
        {
            Debug.LogWarning("Prefab or spawn point is not assigned.");
        }
    }
}