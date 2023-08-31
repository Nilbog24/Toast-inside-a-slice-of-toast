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
    public TextMeshProUGUI infoText;
    public int shots;
    public int buildIndex;
    public int scoreNeeded;
    public Scene currentScene;
    
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene ();  
        buildIndex = currentScene.buildIndex * -1;
        announcementPanel.SetActive(false);
        if(currentScene.name == "PeggleLvl1")
        {
            NewLevel(10, 100);
            infoText.text = "Welcome to the pegging aspect! Press in any direction to shoot! Reach 100 score to beat this level and move on!";
        }
        else if(currentScene.name == "PeggleLvl2")
        {   
            NewLevel(5, 250);
            infoText.text = "Welcome to the second Level! You now have a Dense Orb, meaning it doesn't bounce as much as the old orb, however the density lets it get twice the score! Try it out.";
        }
        else if(currentScene.name == "PeggleLvl3")
        {   
            NewLevel(10, 1000);
            infoText.text = "You've found the MEGA ULTRA PEGGING TOAST CAT! Luckily you've just found an ultra orb, that can bounce and is super dense. Ganbatte!";
        }
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
        SceneManager.LoadScene(buildIndex*-1);
    }

    public void NewLevel(int shotAvailable, int scoreNeededToWin)
    {
        score = 0;
        shots = shotAvailable;
        scoreNeeded = scoreNeededToWin;
        shotsRemaining = shots + 1;
        UpdateScore(0);
        UpdateShots();
        scoreNeededText.text = $"Score Needed to Pass: {scoreNeeded}";
        Debug.Log("Level loaded!");
    }

    void Update()
    {   
        
        if(shotsRemaining == 0 )
        {  
            if(!Turret.instance.currentlyShooting)
            {
                //Debug.Log(buildIndex);
                if(buildIndex == -1)
                {
                    if(score >= scoreNeeded)
                    {
                        SceneManager.LoadScene("PeggleLvl2");
                        Debug.Log("Scene Loaded!");
                        
                    }
                    else
                    {
                        announcementPanel.SetActive(true);
                        announcementText.text = $"I'm sorry, you lost! <br> You needed a score of {scoreNeeded}! <br> You only had a score of {score}! <br>  Would you like to try again?";
                    }
                    
                }
                if(buildIndex == -2)
                {
                    if(score >= scoreNeeded)
                    {
                        SceneManager.LoadScene("PeggleLvl3");
                    }
                    else
                    {
                        announcementPanel.SetActive(true);
                        announcementText.text = $"I'm sorry, you lost! <br> You needed a score of {scoreNeeded}! <br> You only had a score of {score}! <br>  Would you like to try again?";
                    }
                    
                }
                if(buildIndex == -3)
                {
                    if(score >= scoreNeeded)
                    {
                        announcementPanel.SetActive(true);
                        announcementText.text = "Congratulations! You've won! <br> Go back to the menu to restart, or press Quit to quit.";
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
