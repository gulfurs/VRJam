using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Haptics;

public class hapticfeed : MonoBehaviour
{
    [Header("Haptic Clips")]
    public HapticClip hitByEnemyHaptic;         // Haptic clip for getting hit
    public HapticClip hitEnemyHaptic;           // Haptic clip for hitting the enemy
    public HapticClip correctAnswerHaptic;      // Haptic clip for correct answers

    private HapticClipPlayer hapticPlayerRight;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();  // Ensure an AudioSource is attached
    }

    // Call this method when the player gets hit
    public void OnPlayerHit()
    {
        PlayFeedback(hitByEnemyHaptic);
        Debug.Log("OnPlayer");
    }

    // Call this method when the player hits an enemy
    public void OnEnemyHit()
    {
        PlayFeedback(hitEnemyHaptic);
        Debug.Log("OnEnemy");
    }

    // Call this method when the player answers correctly
    public void OnCorrectAnswer()
    {
        PlayFeedback(correctAnswerHaptic);
        Debug.Log("Correct");
    }

    // General method to play the specified haptic clip
    private void PlayFeedback(HapticClip hapticClip)
    {
        // Initialize and play the haptic clip
        hapticPlayerRight = new HapticClipPlayer(hapticClip);
        hapticPlayerRight.Play(Controller.Right);
    }
}
