using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePCController : MonoBehaviour
{
    public float baseSpeed = 10;
    public float baseSpnSpeed = 30;
    public GameObject playerCamera;  // THis is the camera assigned to the player currently
    private Vector3 moveDirection;
    private Quaternion spinDirection;
    private bool isGrounded = false;  // This determines if the object is on the ground or not
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Vertical") >= 0) {
            moveDirection = playerCamera.transform.forward * Time.deltaTime * Input.GetAxis("Vertical") * baseSpeed;
        } else {  // This means that player wants to go backwards
            // going backwards, requires rotation from full frontal
            moveDirection = -transform.up * Time.deltaTime *(baseSpeed/2);
        }
        spinDirection = Quaternion.Euler(new Vector3(0, baseSpnSpeed*Input.GetAxis("Horizontal") * baseSpnSpeed, 0)* Time.deltaTime);
        print(isGrounded);
    }   
    private void FixedUpdate() {  // update adjusted to physics ticks; not limited by framerate
       moveCharacter();
    }
    private void OnCollisionEnter(Collision theCollision) {
        isGrounded = true;
        /*if (theCollision.gameObject.name == "ground") {
            isGrounded = true;
        }*/
    }
    private void OnCollisionExit(Collision theCollision) {
         isGrounded = false;
        /*if (theCollision.gameObject.name == "ground") {
            isGrounded = false;
        }*/
    }
    private void moveCharacter() {  // This checks for any updates to any movement
        if ((moveDirection.x > 0) | (moveDirection.y > 0) | (moveDirection.z > 0)) {
            rb.MovePosition(transform.position + moveDirection);
            //rb.MoveRotation(Quaternion.Slerp(transform.rotation.y, playerCamera.transform.rotation.y, 1));
            //rb.MoveRotation(Quaternion.RotateTowards(playerCamera.transform.rotation.y, transform.rotation.y, 1);
            /*rb.MoveRotation(Quaternion.Euler(new Vector3(0f, 
            Quaternion.Slerp(playerCamera.transform.rotation, 
                transform.rotation, 
                ((playerCamera.transform.rotation.y-transform.rotation.y)/baseSpnSpeed)).eulerAngles.y,
            0f)));
            // Method 2*/
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, playerCamera.transform.eulerAngles.y, transform.eulerAngles.z);
        }
        //rb.MoveRotation(transform.rotation * spinDirection);
    }
}
