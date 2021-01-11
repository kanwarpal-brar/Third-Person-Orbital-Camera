using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float baseSpeed = 10;
    public float baseSpnSpeed = 250;
    public Transform playerCamera;
    private static Vector3 movement;  // Represents current movement vectors
    private static Quaternion cameraY;  // Represents the world space rotation of the object
    private Rigidbody rb;  // This is the object's rigidbody component
    private bool isGrounded;
    private float distToGround;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Below are all the ground checks
        isOnGround();
        // Below are all the movement checks
        cameraY = Quaternion.Euler(0, playerCamera.eulerAngles.y, 0);  // Set the current y angle of the camera
        setMovementVector();
        if (Input.anyKey) {
            // TODO Change anyKey to ONLY movement keys
            rb.transform.rotation = Quaternion.RotateTowards(rb.transform.rotation, cameraY, baseSpnSpeed*Time.deltaTime);
        }
    }
    void FixedUpdate() {  // This does the same job as Update, but is not limited by framerate
        rb.MovePosition(rb.transform.position + transform.TransformDirection(movement) * Time.deltaTime);

    }// Move the character
    private void setMovementVector() {  // Called in update to check control button presses and set movement vector
        movement = new Vector3(baseSpeed*Input.GetAxis("Horizontal"), 0, baseSpeed*Input.GetAxis("Vertical"));  // Input taken in fixedupdate to sync with physics ticks
    }

    private void isOnGround() {  // This function raycasts the ground and returns true or false depending on whether or not the script object is touching any object labelled ground
        RaycastHit hit;
        float distance = 1f;
        Vector3 dir = new Vector3(0, -1);
        if(Physics.Raycast(rb.transform.position, dir, out hit, distance)) {
            isGrounded = true;
        }
        else {
            isGrounded = false;
            distanceToGround();
        }
    }

    private void distanceToGround() {
        RaycastHit hit;
        Vector3 dir = new Vector3(0, -1);
        if((Physics.Raycast(rb.transform.position, dir, out hit, 100000f)) && !isGrounded) {
            distToGround = hit.distance;
        } else {
            distToGround = 0;
        }

    }

}
