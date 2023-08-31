using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class StartGame : MonoBehaviour
{
    public static StartGame instance;

    void Awake()
    {
        instance = this;
    }
    void BeginGame()
    {
        SceneManager.LoadScene(1);
    }
    
}
