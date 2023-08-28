using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PegPop : MonoBehaviour
{
    public static int points = 0;

    private void OnCollisionEnter2D(Collision2D other)
    {
        points += 10;
        Debug.Log("Popped! Total Points: " + points);
        gameObject.SetActive(false);
    }
}
