using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{

    public static Dialogue instance;
    public ELEMENTS elements;

    void Awake()
    {
        instance=this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Say()
    {
        StopSpeaking();

    }

    public void StopSpeaking()
    {
        if(isSpeaking)
        {
            StopCoroutine(speaking);
            speaking = null;
        }
    }
    

    public bool isSpeaking {get{return speaking != null;}}
    Coroutine speaking = null;
    IEnumerator Speaking()
    {
        
    }

    [System.Serializable]
    public class ELEMENTS
    {
        /// <summary>
        /// The main panel containg all dialogue related elements in the UI.
        /// </summary>
        public GameObject speechPanel;
        public Text speakerNameText;
        public Text speechText;
    }
    public GameObject speechPanel {get{return elements.speechPanel;}}
    public Text speakerNameText {get{return elements.speechNameText;}}
    public Text speechText {get{return elements.speechText;}}

}
