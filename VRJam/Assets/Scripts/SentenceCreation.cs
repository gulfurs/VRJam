using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using UnityEngine.InputSystem;

public class SentenceCreation : MonoBehaviour
{
    [System.Serializable]
    public class SentenceData
    {
        [TextArea(2, 5)]
        public string sentenceTemplate; // Sentence with a blank placeholder (e.g., "The quick brown ___ jumps over the lazy dog.")
        public string missingWord; // Missing word for this sentence (e.g., "fox")
    }

    public List<SentenceData> sentences = new List<SentenceData>(); // List of sentences with missing words
    public TextMeshPro sentenceDisplay; // UI Text to display the sentence
    public TextMeshPro revealDisplay; // UI Text to display the revealed letters of the missing word

    //public string transcriptionInput; // The transcription input provided from another script

    private int currentSentenceIndex = 0;
    private int lettersRevealed = 0;
    private SentenceData currentSentence;

    public InputActionProperty RevealLetter_XR; 
    public InputActionProperty ConfirmAction_XR; 
    public UnityConnection unityConnection;

    void OnEnable()
    {
        RevealLetter_XR.action.performed += RevealNextLetter;
        ConfirmAction_XR.action.performed += ConfirmGuess;
    }

    void OnDisable()
    {
        RevealLetter_XR.action.performed -= RevealNextLetter;
        ConfirmAction_XR.action.performed -= ConfirmGuess;
    }

    void Start(){
        LoadCurrentSentence();
    }

    void Update()
    {
        // Reveal letter with 'R' key
        if (Input.GetKeyDown(KeyCode.R))
        {
            RevealNextLetter();
        }

        // Confirm guess with 'C' key
        if (Input.GetKeyDown(KeyCode.C))
        {
            ConfirmGuess(unityConnection.transcription);
        }
    }

    void LoadCurrentSentence(){
        // Load the current sentence and reset revealed letters
        currentSentence = sentences[currentSentenceIndex];
        lettersRevealed = 0;
        UpdateRevealDisplay();
        UpdateSentenceDisplay();
    }

    void UpdateSentenceDisplay(){
        // Display the sentence with the currently revealed letters
        sentenceDisplay.text = currentSentence.sentenceTemplate.Replace("___", revealDisplay.text);
    }

    void UpdateRevealDisplay(){
        // Reveal the part of the missing word up to `lettersRevealed`, fill the rest with underscores
        revealDisplay.text = currentSentence.missingWord.Substring(0, lettersRevealed).PadRight(currentSentence.missingWord.Length, '_');
    }

    private void RevealNextLetter(InputAction.CallbackContext context){
        // Reveal the next letter if any are left
        if (lettersRevealed < currentSentence.missingWord.Length)
        {
            lettersRevealed++;
            UpdateRevealDisplay();
            UpdateSentenceDisplay();
        }
    }

    private void ConfirmGuess(InputAction.CallbackContext context)
    {
        string transcription = unityConnection.transcription;
        // Check if the transcription input matches the missing word
        if (string.IsNullOrEmpty(transcription))
        {
            Debug.Log("Please speak your guess first.");
            return;
        }

       //bool isCorrect = transcription.Equals(currentSentence.missingWord, System.StringComparison.OrdinalIgnoreCase);
        bool isCorrect = transcription.ToLower().Contains(currentSentence.missingWord.ToLower());

        // Result feedback
        if (isCorrect)
        {
            Debug.Log("Correct! The missing word is: " + currentSentence.missingWord);
            NextSentence();
        }
        else
        {
            Debug.Log("Incorrect. Try again.");
        }
    }

    void NextSentence(){
        // Move to the next sentence in the list
        currentSentenceIndex++;
        if (currentSentenceIndex < sentences.Count)
        {
            LoadCurrentSentence();
        }
        else
        {
            Debug.Log("You've completed all sentences!");
            // Add any end game logic here if necessary
        }
    }

}
