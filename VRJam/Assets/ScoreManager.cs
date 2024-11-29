using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    private int currentScore;
    public TextMeshPro scoreText;
    public TextMeshProUGUI lastScoreText, lifeBonusText, hintTaxText, finalScoreText;
    public int hintTax;
    private PlayerHealth playerHP;

    void Start() {
        playerHP = GetComponent<PlayerHealth>();
    }

    public void EndingScore() {
        lastScoreText.text = "Score: " +  currentScore;
        lifeBonusText.text = "Life Bonus: " + playerHP.currentHP;
        hintTaxText.text = "Hint Tax: " + hintTax;
        float finalScore = Mathf.Round((currentScore * playerHP.currentHP) / hintTax);
        finalScoreText.text = "Final Score: " + finalScore; 
    }

    public void AddScore(int value)
    {
        currentScore += value;
        scoreText.text = "Score: " + currentScore.ToString();
        Debug.Log("Score Updated: " + currentScore);
    }

    public void HintTax(){
        hintTax++;
    }
}
