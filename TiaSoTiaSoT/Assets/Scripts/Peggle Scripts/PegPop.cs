using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PegPop : MonoBehaviour
{
    public static int points = 0;
    public GameObject[] toasts;
    public List<GameObject> inactiveToasts = new List<GameObject>();
    public int refresh;

    public bool allGone;

    void Awake()
    {
       toasts = GameObject.FindGameObjectsWithTag("Toast");
       refresh = Random.Range(1, 11);
       allGone = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        points += 10;
        Debug.Log("Popped! Total Points: " + points);
        gameObject.SetActive(false);
        
        
        if(refresh >= 10)
        {
            Refresh();
            Debug.LogError("Power");
        }
        else
        {
            foreach(GameObject toast in toasts)
            {
                if(!toast.activeInHierarchy)
                    inactiveToasts.Add(gameObject);
            }
            
            if(inactiveToasts.Count >= toasts.Length)
            {
                Refresh();
                Debug.LogError("Wow");
            }
            else
                inactiveToasts.Clear();
        }         
    }

    public void Refresh()
    {
        foreach(GameObject toast in toasts)
        {
            toast.SetActive(true);
            refresh = Random.Range(1, 11);
            inactiveToasts.Clear();
        }
        
    }
}
