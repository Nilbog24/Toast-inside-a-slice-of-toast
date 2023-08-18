using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{

    public static DialogueSystem instance;
    public ELEMENTS elements;

    void Awake()
    {
        instance=this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Say(string speech, string speaker = "")
    {
        StopSpeaking();
        
        speaking = StartCoroutine(Speaking(speech, false, speaker));
    }

    /// <summary>
    /// Say Something to be added to what is already on the speech box. 
    /// </summary>
    public void SayAdd(string speech, string speaker = "")
    {
        StopSpeaking();
        SpeechText.text = targetSpeech;
        speaking = StartCoroutine(Speaking(speech, true, speaker));
    }

    public void StopSpeaking()
    {
        if(isSpeaking)
        {
            StopCoroutine(speaking);
            
        }
        speaking = null;
    }
    

    public bool isSpeaking {get{return speaking != null;}}
    [HideInInspector] public bool isWaitingForUserInput = false;

    string targetSpeech = "";
    Coroutine speaking = null;
    IEnumerator Speaking(string speech, bool additive, string speaker = "")
    {
        SpeechLayer.SetActive(true);
        targetSpeech = speech;

        if(!additive)
            SpeechText.text = "";
        else
            targetSpeech = SpeechText.text + targetSpeech;
        CharacterText.text = DetermineSpeaker(speaker); //temporary
        isWaitingForUserInput = false;

        while(SpeechText.text != targetSpeech)
        {
            SpeechText.text += targetSpeech[targetSpeech.Length];
            yield return new WaitForEndOfFrame();
        }

        //Text Finished
        isWaitingForUserInput = true;
        while(isWaitingForUserInput)
            yield return new WaitForEndOfFrame();
        
        StopSpeaking();
    }

    string DetermineSpeaker(string s)
    {
        string retVal = CharacterText.text; //Default return is the current name
        if(s != CharacterText.text && s != "")
            retVal = (s.ToLower().Contains("narrator")) ? "" : s;

        return retVal;
    }

    [System.Serializable]
    public class ELEMENTS
    {
        /// <summary>
        /// The main panel containg all dialogue related elements in the UI.
        /// </summary>
        public GameObject SpeechLayer;
        public Text CharacterText;
        public Text SpeechText;
    }
    public GameObject SpeechLayer {get{return elements.SpeechLayer;}}
    public Text CharacterText {get{return elements.CharacterText;}}
    public Text SpeechText {get{return elements.SpeechText;}}

}
