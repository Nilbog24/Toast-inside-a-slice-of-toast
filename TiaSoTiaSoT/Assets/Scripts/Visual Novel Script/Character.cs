using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   


[System.Serializable]
public class Character 
{
    public string name;
    /// <summary>
    /// The root is the container for all of the images related ot the character in the scene.
    /// </summary>
 
    [HideInInspector] public RectTransform root;
    
    public bool enabled {get{return root.gameObject.activeInHeirarchy;} set{root.gameObject.SetActive(value);}}

    DialogueSystem dialogue;

    /// <summary>
    /// Make this character say something.
    /// </summary>
    /// <param name="speech"></param>
    public void Say(string speech, bool add = false)
    {
        if(!enabled)
            enabled = true;

        if(!add)
            dialogue.Say(speech, characterName);
        else
            dialogue.SayAdd(speech, characterName);
    }

    /// <summary>
    /// Create a new character.
    /// </summary>
    /// <param name="_name"></param>
    public Character(string _name, bool enableOnStart = true)
    {
        CharacterManager cm = CharacterManager.instance;
        // Locate the character prefab.
        GameObject prefab = Resources.Load("Characters/Character["+_name+"]") as GameObject;
        // Spawn an instance of the prefab directily on the character layer.
        GameObject ob = GameObject.Instantiate(prefab, cm.CharacterLayer);

        root = ob.GetComponent<RectTransform> ();
        name = _name;

        renderers.renderer = ob.GetComponentInChildren<RawImage> ();

        dialogue = DialogueSystem.instance;

        enabled = enableOnStart;

    }

    [System.Serializable]
    public class Renderers
    {
        /// <summary>
        /// used as the only image for a single layer character.
        /// </summary>
        public RawImage renderer;
        //Sprites use images
        /// <summary>
        /// The body renderer for a multi layer character.
        /// </summary>
        public Image bodyRenderer;
        /// <summary>
        /// The expression renderer for a multi layer character.
        /// </summary>
        public Image expressionRenderer;
    }

    public Renderers renderers = new Renderers();
}
