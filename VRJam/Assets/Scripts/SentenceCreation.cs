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
    public TextMeshPro GuessDisplay;

    private int currentSentenceIndex = 0;
    //private int lettersRevealed = 0;
    private List<int> revealedIndices = new List<int>();
    private SentenceData currentSentence;

    public InputActionAsset actionAsset;
    //public InputActionProperty RevealLetter_XR;
    //public InputActionProperty ConfirmAction_XR;

    private InputAction revealAction;
    private InputAction confirmAction;
    private InputAction startRec;
    
    public UnityConnection unityConnection;

    void OnEnable()
    {
        var actionMap = actionAsset.FindActionMap("XRActionMap"); 
        revealAction = actionMap.FindAction("RevealLetter_XR");  
        confirmAction = actionMap.FindAction("ConfirmAction_XR"); 
        startRec = actionMap.FindAction("StartRec_XR");

        // Subscribe to the actions
        revealAction.performed += RevealNextLetter;
        confirmAction.performed += ConfirmGuess;
        startRec.performed += StartRecording;

        // Enable the actions
        revealAction.Enable();
        confirmAction.Enable();
        startRec.Enable();
    }

    void OnDisable()
    {
        revealAction.performed -= RevealNextLetter;
        confirmAction.performed -= ConfirmGuess;
        startRec.performed -= StartRecording;

        // Disable the actions
        revealAction.Disable();
        confirmAction.Disable();
        startRec.Disable();
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
        //lettersRevealed = 0;
        revealedIndices.Clear();
        UpdateRevealDisplay();
        UpdateSentenceDisplay();
    }

    void UpdateSentenceDisplay()
    {
        sentenceDisplay.text = currentSentence.sentenceTemplate.Replace("___", revealDisplay.text);
    }

    void UpdateRevealDisplay()
    {
        //revealDisplay.text = currentSentence.missingWord.Substring(0, lettersRevealed).PadRight(currentSentence.missingWord.Length, '_');
        char[] revealArray = new string('_', currentSentence.missingWord.Length).ToCharArray();
        
        // Place revealed letters at their indices
        foreach (int index in revealedIndices)
        {
            revealArray[index] = currentSentence.missingWord[index];
        }

        revealDisplay.text = new string(revealArray);
    }

    // Method for Input System callbacks
    private void RevealNextLetter(InputAction.CallbackContext context)
    {
        RevealNextLetterDirect();
    }

    // Direct method for legacy input
    private void RevealNextLetterDirect()
    {
        if (revealedIndices.Count < currentSentence.missingWord.Length)
        {
            int randomIndex;
            // Pick a random index that hasn't been revealed yet
            do
            {
                randomIndex = Random.Range(0, currentSentence.missingWord.Length);
            }
            while (revealedIndices.Contains(randomIndex));

            revealedIndices.Add(randomIndex);

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
            GuessDisplay.text = "Please speak your guess first.";
            return;
        }

        bool isCorrect = transcription.ToLower().Contains(currentSentence.missingWord.ToLower());

        if (isCorrect)
        {
            Debug.Log("Correct! The missing word is: " + currentSentence.missingWord);
            GuessDisplay.text = "Correct! The missing word is: " + currentSentence.missingWord;
            NextSentence();
        }
        else
        {
            Debug.Log("Incorrect. Try again.");
            GuessDisplay.text = "Incorrect. Try again.";
            Enemy enemyController = FindObjectOfType<Enemy>();
            if (enemyController != null){
                    enemyController.ReleaseEnemy();
                }
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
            GuessDisplay.text = "You've completed all sentences!";
        }
    }
    private void StartRecording(InputAction.CallbackContext context)
    {
        unityConnection.StartTranscriptionProcess();
    }
    
}