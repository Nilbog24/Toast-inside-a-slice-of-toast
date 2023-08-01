using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The majority of this code was taken from the below tutorial, made by MoreBBlakeyyy
// https://www.youtube.com/watch?v=-bkmPm_Besk&ab_channel=MoreBBlakeyyy 
public class Projectile : MonoBehaviour
{
    // This will be used to store the mouse's position.
    private Vector3 mousePos;
    // This will be used to store the camera object.
    private Camera mainCam;
    // This will later be assigned the projectiles rigidbody so the script can change it's values.
    private Rigidbody2D rb;
    // This is used to determine the amount of force that the projectile spawned will have.
    public float force;
    // This gets the gamemanager object so that variables and methods from it can be used in this script.
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        //  This will assign the camera component of the main camera to the maincam variable.
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        // This'll assign the rigidbody to the rb variable.
        rb = GetComponent<Rigidbody2D>();
        // This'll make the mousePos wariable equal to the mouses position.
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        // These two vector three's will determine the direction and rotaion of the projectile respectively.
        Vector3 direction = mousePos - transform.position;
        Vector3 rotation = transform.position - mousePos;
        // This will change the velocity of the rigid body to be a vector two pointing towards the mouse.
        rb.velocity = new Vector2(direction.x, direction.y).normalized * force;
        // This float will be the direction that the projectile should face.
        float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        // This will make it so that the projectile will face towards the mouse.
        // There's the plus one eighty because the arrow projectile prefab is facing the wrong way.
        transform.rotation = Quaternion.Euler(0, 0, rot + 180);
        // This'll get the GameManager script and assign it to the gameManager variable.
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    // This method is only used to destroy the projectile once it reaches out of bounds.
    // This is so theren't tons of entites existing.
    void Update()
    {
        if (transform.position.x > 12 || transform.position.y > 5)
        {
            Destroy(gameObject);
        }
        if (transform.position.x < -12 || transform.position.y < -5)
        {
            Destroy(gameObject);
        }
    }
    
    // This method is used to determine when the projectile collides with another object.
    void OnTriggerEnter2D(Collider2D other)
    {
        // If the other object is an enemy then both the projectile and the monster wiil be destroyed.
        // Then the UpdateScore method from the game manager will be called and the score will increase by one.
        if (other.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            Destroy(other.gameObject);
            gameManager.UpdateScore(1);
        }
        // If the other object isn't an enemy, a player, or another arrow then it's velocity will become zero.
        // After a second the projectile will be destroyed.
        // The reason it can't be the player is that the projectile spawns on the player.
        // If the not player wasn't there then the projectile would immediatly lose all velocity and die. 
        else if (!other.CompareTag("Player") && !other.CompareTag("Arrow"))
        {
            rb.velocity = Vector3.zero;
            Destroy(gameObject, 1);
        }
    }
}
