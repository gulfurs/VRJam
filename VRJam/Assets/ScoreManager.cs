using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    private int currentScore;
    public TextMeshPro scoreText;
    
    public void AddScore(int value)
    {
        currentScore += value;
        scoreText.text = "Score: " + currentScore.ToString();
        Debug.Log("Score Updated: " + currentScore);
    }
}
