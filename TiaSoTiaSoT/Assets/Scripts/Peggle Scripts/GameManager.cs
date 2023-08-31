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
    public TextMeshProUGUI scoreNeededText;
    public TextMeshProUGUI announcementText;
    public GameObject announcementPanel;
    public int shots;
    public int buildIndex;
    public int scoreNeeded;
    void Awake()
    {
        instance = this;
        score = 0;
        shots = 10;
        scoreNeeded = 100;
        shotsRemaining = shots + 1;
        UpdateScore(0);
        UpdateShots();
        scoreNeededText.text = $"Score Needed to Pass: {scoreNeeded}";
    }
    public Scene currentScene;
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene ();    
        announcementPanel.SetActive(false);
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

    public void Reload()
    {
        SceneManager.LoadScene(buildIndex);
    }

    void Update()
    {   
        buildIndex = currentScene.buildIndex * -1;
        if(shotsRemaining == 0 )
        {  
            Debug.Log("No more shots");
            if(!Turret.instance.currentlyShooting)
            {
                Debug.Log(buildIndex);
                if(buildIndex == 1)
                {
                    if(score >= scoreNeeded)
                    {
                        SceneManager.LoadScene(2);
                        shots = 5;
                        Scene currentScene = SceneManager.GetActiveScene ();
                        scoreNeeded = 125;
                        scoreNeededText.text = $"Score Needed to Pass: {scoreNeeded}";
                    }
                    else
                    {
                        announcementPanel.SetActive(true);
                        announcementText.text = $"I'm sorry, you lost! <br> You needed a score of {scoreNeeded}! <br> You only had a score of {score}! <br>  Would you like to try again?";
                    }
                    
                }
                if(buildIndex == 2)
                {
                    if(score >= scoreNeeded)
                    {
                        SceneManager.LoadScene(3);
                        shots = 12;
                        Scene currentScene = SceneManager.GetActiveScene ();
                    }
                    else
                    {
                        announcementPanel.SetActive(true);
                        announcementText.text = $"I'm sorry, you lost! <br> You needed a score of {scoreNeeded}! <br> You only had a score of {score}! <br>  Would you like to try again?";
                    }
                    
                }
            }
            
        }
    }
}
