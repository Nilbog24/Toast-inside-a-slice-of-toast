using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PegPop : MonoBehaviour
{
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
        GameManager.instance.UpdateScore(1);
        gameObject.SetActive(false);
        
        
        if(refresh >= 10)
        {
            Refresh();
            Debug.Log("Power");
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
                Debug.Log("Wow");
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
        }
        inactiveToasts.Clear();
    }
}
