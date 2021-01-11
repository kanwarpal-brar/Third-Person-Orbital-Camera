using System.Threading;
using System;
using System.IO.Compression;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCController : MonoBehaviour
{
    // Variables are declared before start, not in start
    // Variables are initilized in start
    Rigidbody rigidBody;
    public float baseSpeed;
    public float turningSpeed;
    public float turnAcceleration;
    private float timeTurning;  // This variable allows calculation of accelleration
    CharacterController charControl;
     Vector3 reposDelta;  // Represents the position offset from starting position
     Vector3 rotationVelocity;
    // Start is called before the first frame update

    void Start()
    {
        // Retrive charactercontroller element (not used anymore)
        // charControl = GetComponent<CharacterController>();
        // Retrieve rigidbody   
        rigidBody = GetComponent<Rigidbody>();
        // baseSpeed = 5.0f;  // Float representing base speed
        //turningSpeed = 500;
        
    }

    // Update is called once per frame
    void Update()
    {
        rotationVelocity = new Vector3(0, turningSpeed + calcTurnAcceleration(), 0);
        /*Vectors are defined as:
            new Vector(x, y, z)
        */
        if (Input.GetButton("Vertical")) {
            // This code is executed if a bound horizontal/vertical access button is pressed
            // MovePosition is not relative to the orientation and position of the rigid collider, it is global; position must be referenced explicitly
            rigidBody.MovePosition(transform.position + transform.forward * Time.deltaTime * baseSpeed);
            // Linear operations are multipled by deltatime to make they fixed to time instead of framerate
        }
        if (Input.GetButton("Horizontal")) {
            timeTurning = Time.time;
            // This rotates at about 15 degrees/fps * turningSpeed when the + or - horizontal axis is on
            rigidBody.MoveRotation(rigidBody.rotation * Quaternion.Euler(rotationVelocity * Time.deltaTime * Input.GetAxis("Horizontal")));
        }
        if (Input.GetButtonUp("Horizontal")) {
            timeTurning = 0;
        }
        
    }
    float calcTurnAcceleration() {
        // Calculates how long the turn button has been held and uses it to calculate the accelleration
        if (timeTurning != 0) {
            return turnAcceleration * (Time.time - timeTurning);
        } else {
            return 0;
        }
    }
}
