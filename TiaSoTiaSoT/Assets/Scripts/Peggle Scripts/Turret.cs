using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The majority of this code was taken from the below tutorial, made by MoreBBlakeyyy
// https://www.youtube.com/watch?v=-bkmPm_Besk&ab_channel=MoreBBlakeyyy 
public class Turret : MonoBehaviour
{
    // This will be used to store the camera object.
    private Camera mainCam;
    // This will be used to store the mouse's position.
    private Vector3 mousePos;
    // This will be used to store the projectile prefab so it can be instantiated later.
    public GameObject projectilePrefab;
    // This gets the position that the arrow's going to be spawned at.
    public Transform projectileTransform;
    // This boolean determines whether or not the player can fire.
    public bool canFire;
    public bool currentlyShooting = false;
    private GameManager gameManager;
    

    public Animator animator;

    // Start is called before the first frame update
    // In this method the mainCam variable will be assigned the camera component
    // Then the playerControllerScript variable will be assigned the PlayerControll script.
    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        // This'll set the mousePos variable to the mouse's position.
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        
        // These three lines of code will get the wanted rotation.
        Vector3 rotation = mousePos - transform.position;

        float rotationZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, rotationZ+90);

        // This chunk of code in this if loop is the code that create the cooldown between shots.
        // First if canFire is false then the timer variable will increase with the change in time.
        // Then if timer is greater than the cooldown length the canFire will become true and the timer will be reset.
        if(!canFire)
        {
            if(!currentlyShooting)// Figure out how to check the tag of the background, if the tag is Peggle or something then can fire = true
            {
                canFire = true;
            }
        }
        
        // If the player does a left click and canFire is true then canFire will become false.
        // Then the projectile will be created.
        if(Input.GetKeyDown(KeyCode.Mouse0) && canFire)
        {
            animator.SetTrigger("Shoot");
            
            canFire = false;
            Instantiate(projectilePrefab, projectileTransform.position, Quaternion.identity);
            currentlyShooting = true;
            

        }
    }
}
