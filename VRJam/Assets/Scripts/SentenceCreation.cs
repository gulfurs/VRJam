using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using System;

public class SentenceCreation : MonoBehaviour
{

    // The SentenceData
    [System.Serializable]
    public class SentenceData
    {
        [TextArea(2, 5)]
        public string sentenceTemplate;
        public string missingWord;
        public string category;
        [TextArea(2, 5)]
        public string definition;
    }

    //The TV Screen Text
    public List<SentenceData> sentences = new List<SentenceData>();

    public TextMeshPro revealDisplay;
    public TextMeshPro GuessDisplay;
    public TextMeshPro answerDisplay;

    public TextMeshPro categoryDisplay;
    public TextMeshPro definitionDisplay;
    public TextMeshPro sentenceDisplay;
    //public TextMeshPro useInSentence;

    //Bool checker for TV
    private bool isDefinition = false;
    private bool isUseInSentece = false;


    //SentenceLogic
    private int currentSentenceIndex = 0;
    private List<int> revealedIndices = new List<int>();
    private SentenceData currentSentence;

    //InputActions
    public InputActionAsset actionAsset;
    private InputAction revealAction;
    private InputAction confirmAction;

    private InputAction HintDef;
    private InputAction HintSen;
    private InputAction startRec;
    
    // Reference to other scripts
    public UnityConnection unityConnection;
    private AudioManager audioManager;
    public Enemy enemyController;
    public GameObject Sword;

    private ScoreManager getScoreManager;
    //Fighting
    public event Action winEvent;
    public event Action loseEvent;

    private int maxScore = 100;
    private int score = 100;
    private bool enemySummoned = false;

    void OnEnable()
    {
        var actionMap = actionAsset.FindActionMap("XRActionMap"); 
        revealAction = actionMap.FindAction("RevealLetter_XR");  
        confirmAction = actionMap.FindAction("ConfirmAction_XR"); 

        HintDef = actionMap.FindAction("HintDef"); 
        HintSen = actionMap.FindAction("HintSen"); 

        startRec = actionMap.FindAction("StartRec_XR");

        // Subscribe to the actions
        revealAction.performed += RevealNextLetter;
        confirmAction.performed += ConfirmGuess;
        //startRec.performed += StartRecording;
        
        HintDef.performed += DefinitionHint;
        HintSen.performed += SentenceHint;

        // Enable the actions
        revealAction.Enable();
        confirmAction.Enable();

        HintDef.Enable();
        HintSen.Enable();
        startRec.Enable();
    }

    void OnDisable()
    {
        revealAction.performed -= RevealNextLetter;
        confirmAction.performed -= ConfirmGuess;
        //startRec.performed -= StartRecording;
    
        HintDef.performed -= DefinitionHint;
        HintSen.performed -= SentenceHint;

        // Disable the actions
        revealAction.Disable();
        confirmAction.Disable();

        HintDef.Disable();
        HintSen.Disable();
        startRec.Disable();
    }

    void Start()
    {   
        getScoreManager = FindObjectOfType<ScoreManager>();
        audioManager = AudioManager.instance;
        enemyController = FindObjectOfType<Enemy>();
        ShuffleList();
        LoadCurrentSentence();
        if (audioManager == null){Debug.LogError("AudioManager instance not found.");}
        //if (enemyController == null){Debug.LogError("Enemy controller not found.");}
    }

    void Update()
    {
        // Handle legacy input
        if (Input.GetKeyDown(KeyCode.R))
        {
            RevealNextLetterDirect();
            //loseEvent?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            ConfirmGuessDirect();
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            ToggleDefinitionHint();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            //ToggleSentenceHint();
            CorrectAnswer();
        }

    }


    void LoadCurrentSentence()
    {
        currentSentence = sentences[currentSentenceIndex];
        score = maxScore;
        isDefinition = false;
        isUseInSentece = false;
        revealedIndices.Clear();
        revealedIndices.Add(0);
        UpdateRevealDisplay();
        UpdateSentenceDisplay();

        //The hints
        categoryDisplay.text = "Category: " + currentSentence.category;
        definitionDisplay.text ="Get Hint";
        sentenceDisplay.text ="Get Hint";
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
                randomIndex = UnityEngine.Random.Range(0, currentSentence.missingWord.Length);
            }
            while (revealedIndices.Contains(randomIndex));

            revealedIndices.Add(randomIndex);
            score = Mathf.CeilToInt(maxScore * (1 - (float)revealedIndices.Count / currentSentence.missingWord.Length));
            score = Mathf.Max(score, 0);
            //Debug.Log(revealedIndices.Count);
            UpdateRevealDisplay();
            //UpdateSentenceDisplay();
        }
        if (revealedIndices.Count == currentSentence.missingWord.Length)
        {
            loseEvent?.Invoke();
        }
    }

    public void ToggleDefinitionHint(){
        if (!isDefinition)
        {
        definitionDisplay.text = "Definition: " + currentSentence.definition;
        getScoreManager.HintTax();
        isDefinition = true;
        }
    }
    public void ToggleSentenceHint(){
        if (!isUseInSentece)
        {
        sentenceDisplay.text = "Sentence: " + currentSentence.sentenceTemplate;
        getScoreManager.HintTax();
        isUseInSentece = true;
        }
    }
    
    private void DefinitionHint(InputAction.CallbackContext context)
    {
        ToggleDefinitionHint();
    }
    private void SentenceHint(InputAction.CallbackContext context)
    {
        ToggleSentenceHint();
    } 

    // Method for Input System callbacks
    private void ConfirmGuess(InputAction.CallbackContext context)
    {
        ConfirmGuessDirect();
    }

    // Direct method for legacy input
    private void ConfirmGuessDirect()
    {
        if (enemySummoned && enemyController != null && enemyController.IsDead())
        {
            NextSentence();
            return;
        }

        string transcription = answerDisplay.text;
        if (string.IsNullOrEmpty(transcription))
        {
            Debug.Log("Please speak your guess first.");
            GuessDisplay.text = "Please speak your guess first.";
            return;
        }

        bool isCorrect = transcription.ToLower().Contains(currentSentence.missingWord.ToLower());

        if (isCorrect)
        {
            CorrectAnswer();
        }
        else
        {
            WrongAnswer();
        }
    }

    public void NextSentence()
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
            var getGameManager = FindObjectOfType<GameManager>();
            getGameManager.EndGame();
        }
    }

    void CorrectAnswer(){
        winEvent?.Invoke();
        Debug.Log("Correct! The missing word is: " + currentSentence.missingWord);
        GuessDisplay.text = "Correct! The missing word is: " + currentSentence.missingWord;
        audioManager.PlayRight();
        getScoreManager.AddScore(score);
        NextSentence();
    }

    void WrongAnswer() {
        loseEvent?.Invoke();
        Debug.Log("Incorrect. Try again.");
        GuessDisplay.text = "Incorrect. Try again.";
        audioManager.PlayWrong();
    }

    public void ShuffleList()
    {
        for (int i = 0; i < sentences.Count; i++)
        {
            SentenceData temp = sentences[i];
            int randomIndex = UnityEngine.Random.Range(i, sentences.Count);
            sentences[i] = sentences[randomIndex];
            sentences[randomIndex] = temp;
        }
    }
}