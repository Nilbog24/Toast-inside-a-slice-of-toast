using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int score;
    public int shotsRemaining;

    public TextMeshProUGUI scoreText;

    public TextMeshProUGUI shotsRemainingText;
    public int shots;
    public int buildIndex;
    void Awake()
    {
        instance = this;
        score = 0;
        shots = 10;
        shotsRemaining = shots;
        UpdateScore(0);
    }
    public Scene currentScene;
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene ();    
    }

    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score;
        Debug.Log("Popped! Total Points: " + score);
    }

    public void UpdateShots()
    {
        shotsRemaining--;
        shotsRemainingText.text = "Shots Remaining: " + shotsRemaining;
        Debug.Log("Shot! Shots Remaining: " + shotsRemaining);
    }

    void Update()
    {   
        buildIndex = currentScene.buildIndex;
        if(shotsRemaining == 0 && !Turret.instance.currentlyShooting)
        {
            if(buildIndex == 1)
            {
                SceneManager.LoadScene(2);
                shots = 12;
                Scene currentScene = SceneManager.GetActiveScene ();
            }
            if(buildIndex == 2)
            {
                SceneManager.LoadScene(3);
                shots = 12;
                Scene currentScene = SceneManager.GetActiveScene ();
            }
        }
    }
}
