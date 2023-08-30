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
    // Start is called before the first frame update

    private Turret turret;
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

        turret = GameObject.FindGameObjectWithTag("Turret").GetComponent<Turret>();
    }

    // Update is called once per frame
    // This method is only used to destroy the projectile once it reaches out of bounds.
    // This is so theren't tons of entites existing.
    void Update()
    {
        Vector2 retainedVelocity = rb.velocity;
        while(Menu.instance.isPause)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        rb.constraints = RigidbodyConstraints2D.None;
        // rb.velocity = retainedVelocity;
        
        if (transform.position.x > 16)
        {
            Destroy(gameObject);
            turret.currentlyShooting = false;
        }
        if (transform.position.x < -16 || transform.position.y < -5)
        {
            Destroy(gameObject);
            turret.currentlyShooting = false;
        }
    }
}
