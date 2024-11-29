using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject leaderBoard;

    public void EndGame()
    {
        Debug.Log("Game Over!");
        ShowLeaderboard();
    }

    private void ShowLeaderboard()
    {
        Debug.Log("Showing Leaderboard...");
    }
}
