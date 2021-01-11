using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class KinematicPlayerController : MonoBehaviour
{
    public float baseSpeed = 4;
    public float runSpeed = 10;
    public float stamina = 100;
    public float baseSpnSpeed = 420;
    public float fallSpeed = 10;
    public float gravity = 9;
    public float slopeForce = 5;
    public float slopeForceRayLength = 4;
    public Transform playerCamera;
    //private static Vector3 movement;  // Represents current movement vectors
    private static Quaternion cameraY;  // Represents the world space rotation of the object
    private CharacterController cc;  // This is the object's rigidbody component
    private float distToGround;
    public Animator anim;  // Represents the animator in charge of the player character
    private bool isRunning = false;
    //private int turning = 0;  // Represents the turning state of the character. -1 is to the left, 1 is to the right, 0 is no turning applied [NOT CURRENTLY USED]

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        playerCamera = transform.Find("CameraTracker").Find("Camera");  // Looks for a child camera of the child CameraTracker object
    }

    // Update is called once per frame
    void Update()
    {
        // Below are all the movement checks
        playerRotation();
        playerMovement();
        playerAnimation();
    }
    void FixedUpdate()
    {
        /*
        // Below are all operation related keeping the object attached to the ground
        distanceToGround();
        if (!cc.isGrounded)  // Check if object is on ground, if not, perform operation
        {
            cc.Move(new Vector3(0, -distToGround, 0) * Time.deltaTime * fallSpeed);
        }


        // Below are all movement operations
        cc.Move(transform.TransformDirection(movement) * Time.deltaTime);  // Transforms relative to current position
        */
        //playerMovement();  // Calls the method handling movement of the character controller
    }// Move the character
    private void playerMovement()
    {
            // Below are all commands managing the movement of the character
        Vector3 movement;
        if (isRunning)
        {
            movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized * runSpeed;
        }
        else
        {
            movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized * baseSpeed;
        }
        cc.SimpleMove(playerCamera.transform.TransformDirection(movement));  // Performs the simple forward movement on the character

        // Below operations check to see if the player is traveling down a slope. Extra gravitational force must be applied to prevent bouncing
        if (((Input.GetAxis("Horizontal") !=0 || Input.GetAxis("Vertical") !=0 )) && onSlope())
        {
            cc.Move(Vector3.down * cc.height/2*slopeForce * Time.deltaTime);
        }
    }
    private void playerRotation() // Manages the rotation of the character
    {
        // Below code handles the rotation of the player character in accordance with the player camera
        cameraY = Quaternion.Euler(0, playerCamera.eulerAngles.y, 0);  // Set the current y angle of the camera
        float speed = baseSpnSpeed;
        if (isRunning)
        {
            speed *= 2;
        }
        if (Input.GetAxis("Vertical")!=0 && Input.GetAxis("Horizontal")==0)
        {
            Quaternion newRotAngle = Quaternion.Euler(0, cameraY.eulerAngles.y + Mathf.Rad2Deg * Mathf.Atan(Input.GetAxis("Horizontal") / Input.GetAxis("Vertical")), 0);
            cc.transform.rotation = Quaternion.RotateTowards(cc.transform.rotation, newRotAngle, speed * Time.deltaTime);  // Perform the new rotation
        }
        // The Below is a manual system for controlling rotation direction. I am attempting to replace it with one based on trig ratios
        /*
        if (Input.GetAxis("Vertical") > 0 && Input.GetAxis("Horizontal") == 0)  // If the forward movement key is pressed
        {
            cc.transform.rotation = Quaternion.RotateTowards(cc.transform.rotation, cameraY, speed * Time.deltaTime);  // Perform the new rotation
        }
        else if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") > 0)
        {
            Quaternion sideAngle = Quaternion.Euler(new Vector3(cc.transform.rotation.x, cameraY.eulerAngles.y + 90, cc.transform.rotation.z));
            cc.transform.rotation = Quaternion.RotateTowards(cc.transform.rotation, sideAngle, speed * Time.deltaTime);
        }
        else if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") < 0)
        {
            Quaternion sideAngle = Quaternion.Euler(new Vector3(cc.transform.rotation.x, cameraY.eulerAngles.y - 90, cc.transform.rotation.z));
            cc.transform.rotation = Quaternion.RotateTowards(cc.transform.rotation, sideAngle, speed * Time.deltaTime);
        }*/
    }
    private void playerAnimation()
    {
        if (Input.GetKeyDown("left shift"))  // Check if running
        {
            print("Got it");
            isRunning = !isRunning;  // Toggles running
        }
        if (isRunning)
        {
            anim.SetFloat("Moving", Mathf.Abs(Input.GetAxis("Vertical")) + Mathf.Abs(Input.GetAxis("Horizontal"))+1);
        } else
        {
            anim.SetFloat("Moving", Mathf.Abs(Input.GetAxis("Vertical"))+Mathf.Abs(Input.GetAxis("Horizontal")));
        }
    }

    private void distanceToGround()
    {
        RaycastHit hit;
        Vector3 dir = new Vector3(0, -1);
        if ((Physics.Raycast(cc.transform.position, dir, out hit, 100000f)) && !cc.isGrounded)
        {
            distToGround = hit.distance;
        }
        else
        {
            distToGround = 0;
        }

    }

    private bool onSlope()  // A function designed to test if the player character is on a slope
    {
        RaycastHit hit;
        if (Physics.Raycast(cc.transform.position, Vector3.down, out hit, cc.height / 2 * slopeForceRayLength))
        {
            if (hit.normal != Vector3.up)  // If the normal vector is not horizontal
            {
                return true;
            }
        }
        return false;
    }
}
