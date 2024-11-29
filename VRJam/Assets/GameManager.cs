using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameManager : MonoBehaviour
{
    private PlayableDirector playDirect;
    public ScoreManager scoreManager;

    void Start()
    {
        playDirect = GetComponent<PlayableDirector>();
        //scoreManager = GetComponent<ScoreManager>();
    }

    public void EndGame()
    {
        Debug.Log("Game Over!");
        if (playDirect != null)
        {
            playDirect.Play();
        }
        scoreManager.EndingScore();
    }
}
