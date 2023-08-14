using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for adding and maintaining  characters in the scene.
/// </summary>
public class CharacterManager : MonoBehaviour
{

    public static CharacterManager instance;

    /// <summary>
    /// All characters must be attached to the character layer
    /// </summary> 
    public RectTransform CharacterLayer;
    
    /// <summary>
    /// A list of all characters currently in the scene.
    /// </summary>
    public List<Character> characters = new List<Character>();
    
    public Dictionary
    void Awake()
    {
        instance = this;
    }
}
