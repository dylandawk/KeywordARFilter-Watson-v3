using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using IBM.Watson.SpeechToText.V1;

public class KeywordTrigger : MonoBehaviour
{
    [SerializeField]
    SpeechToText speechToText;

    [SerializeField]
    private string keyword;
    [SerializeField]
    int maxCharacters;
    [SerializeField]
    InputField inputField;
    [SerializeField]
    Text characterLimit;
    private int keywordCount = 0;

    //[SerializeField]
    //Text keywordText;
    //[SerializeField]
    //Text recordedText;

    private bool isKeywordRecorded;

    public UnityEvent KeywordTriggered;
    private SpeechRecognitionEvent results;
    

    private void Start()
    {
        if(KeywordTriggered == null ) KeywordTriggered = new UnityEvent();
        speechToText.speechRecognized.AddListener(CheckForKeyword);

        SetupInputField();
    }

    private void Update()
    {
       
    }

    private void SetupInputField()
    {
        inputField.characterLimit = maxCharacters;
        characterLimit.text = "0/" + inputField.characterLimit;
    }

    public void UpdateKeywordCount(string currentInput)
    {

        keywordCount = currentInput.Length;
        characterLimit.text = keywordCount.ToString() + "/" + inputField.characterLimit;
        if (keywordCount == maxCharacters)
        {
            characterLimit.color = new Color(1f, .004f, .35f);
            Debug.Log("Max Character count reached");
        }
        else characterLimit.color = Color.white;
    }

    

    public void UpdateKeyword(string newKeyword)
    {
        keyword = newKeyword.ToLower();
        //keywordText.text = keyword;
    }

    private void CheckForKeyword(SpeechRecognitionEvent result)
    {
        if (result != null && result.results.Length > 0)
        {
            foreach (var res in result.results)
            {
                foreach (var alt in res.alternatives)
                {
                    // make text lower case
                    string transcript = alt.transcript.ToLower();
                    if (transcript.Contains(keyword))
                    {
                        Debug.Log("IsKeywordRecorded: " + isKeywordRecorded);
                        // if keyword has not yet been triggered trigger
                        if (!isKeywordRecorded)
                        {
                            Debug.Log("Triggered: " + keyword);
                            //keywordText.text = keyword;
                            //keywordText.color = Random.ColorHSV();
                            KeywordTriggered.Invoke();
                            isKeywordRecorded = true;
                        }
                        else if (res.final) isKeywordRecorded = false;
                    }
                }
            }
        }
    }
}
