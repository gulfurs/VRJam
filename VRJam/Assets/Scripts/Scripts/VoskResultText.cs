using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VoskResultText : MonoBehaviour 
{
    public VoskSpeechToText VoskSpeechToText;
    public TextMeshPro ResultText;
    public bool CheckInput = false;
    void Awake()
    {
        VoskSpeechToText.OnTranscriptionResult += OnTranscriptionResult;
    }

    private void OnTranscriptionResult(string obj)
    {
        //Debug.Log(obj);
        if (CheckInput) {
        var result = new RecognitionResult(obj);
        ResultText.text = "";
        for (int i = 0; i < result.Phrases.Length; i++)
        {
            if (i > 0)
            {
                ResultText.text += ", ";
            }

            ResultText.text += result.Phrases[i].Text;
        }
    	ResultText.text += "\n";
        }
    }
}
