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
        public string sentenceTemplate;
        public string missingWord;
    }

    public List<SentenceData> sentences = new List<SentenceData>();
    public TextMeshPro sentenceDisplay;
    public TextMeshPro revealDisplay;

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

    void Start()
    {
        LoadCurrentSentence();
    }

    void Update()
    {
        // Handle legacy input
        if (Input.GetKeyDown(KeyCode.R))
        {
            RevealNextLetterDirect();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            ConfirmGuessDirect();
        }
    }

    void LoadCurrentSentence()
    {
        currentSentence = sentences[currentSentenceIndex];
        lettersRevealed = 0;
        UpdateRevealDisplay();
        UpdateSentenceDisplay();
    }

    void UpdateSentenceDisplay()
    {
        sentenceDisplay.text = currentSentence.sentenceTemplate.Replace("___", revealDisplay.text);
    }

    void UpdateRevealDisplay()
    {
        revealDisplay.text = currentSentence.missingWord.Substring(0, lettersRevealed).PadRight(currentSentence.missingWord.Length, '_');
    }

    // Method for Input System callbacks
    private void RevealNextLetter(InputAction.CallbackContext context)
    {
        RevealNextLetterDirect();
    }

    // Direct method for legacy input
    private void RevealNextLetterDirect()
    {
        if (lettersRevealed < currentSentence.missingWord.Length)
        {
            lettersRevealed++;
            UpdateRevealDisplay();
            UpdateSentenceDisplay();
        }
    }

    // Method for Input System callbacks
    private void ConfirmGuess(InputAction.CallbackContext context)
    {
        ConfirmGuessDirect();
    }

    // Direct method for legacy input
    private void ConfirmGuessDirect()
    {
        string transcription = unityConnection.transcription;
        if (string.IsNullOrEmpty(transcription))
        {
            Debug.Log("Please speak your guess first.");
            return;
        }

        bool isCorrect = transcription.ToLower().Contains(currentSentence.missingWord.ToLower());

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

    void NextSentence()
    {
        currentSentenceIndex++;
        if (currentSentenceIndex < sentences.Count)
        {
            LoadCurrentSentence();
        }
        else
        {
            Debug.Log("You've completed all sentences!");
        }
    }
}