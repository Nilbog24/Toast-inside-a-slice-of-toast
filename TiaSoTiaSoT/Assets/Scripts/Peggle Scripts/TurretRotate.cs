using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretRotate : MonoBehaviour
{
    // This will be used to store the camera object.
    private Camera mainCam;
    // This will be used to store the mouse's position.
    private Vector3 mousePos;
    // Start is called before the first frame update
    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        // This'll set the mousePos variable to the mouse's position.
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        if(mousePos.y !< 4.7)
        {
            // These three lines of code will get the wanted rotation.
            Vector3 rotation = mousePos - transform.position;

            float rotationZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg + 90;

            transform.rotation = Quaternion.Euler(0, 0, rotationZ);
        }
        
           
    }
}
