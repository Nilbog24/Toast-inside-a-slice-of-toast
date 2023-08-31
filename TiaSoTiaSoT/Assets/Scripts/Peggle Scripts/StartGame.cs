using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class StartGame : MonoBehaviour
{
    public static StartGame instance;
    public bool isLevelOne;

    void Awake()
    {
        instance = this;
    }
    void BeginGame()
    {
        isLevelOne = true;
        SceneManager.LoadScene(1);
    }
    
}
