using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsController : MonoBehaviour
{
    private Rigidbody playerBody;
    private Vector3 playerVelocity;  // This is used in the transformation
    private Vector3 playerRotVelocity;
    public float baseSpeed = 5;  // This variable controls the base movement speed
    public float baseSpinSpeed = 150; // This variable controls the rotational speed
    // Start is called before the first frame update
    void Start()
    {
        playerBody = GetComponent<Rigidbody>();  // This retrieves a rigidbody component of the object
    }

    // Update is called once per frame
    void Update()
    {
        //transform.localPosition = transform.TransformDirection(new Vector3(baseSpeed*Input.GetAxis("Vertical"), playerBody.velocity.y, playerBody.velocity.z));
        //playerBody.velocity = new Vector3(baseSpeed*Input.GetAxis("Vertical"), playerBody.velocity.y, playerBody.velocity.z);  // Vectors are x, y, z components, in that order
        //playerBody.angularVelocity = new Vector3(0, baseSpinSpeed*Input.GetAxis("Horizontal"), 0);
        playerVelocity = transform.TransformDirection(new Vector3(baseSpeed*Input.GetAxis("Vertical"), playerBody.velocity.y, playerBody.velocity.z));
        playerRotVelocity = new Vector3(0, baseSpinSpeed*Input.GetAxis("Horizontal"), 0);
    }
    void FixedUpdate() {  // All physics operations should be performed here, it goes by physics step instead of by frames
        playerBody.MovePosition(transform.position + playerVelocity * Time.deltaTime);
        playerBody.MoveRotation(transform.rotation * Quaternion.Euler(playerRotVelocity * Time.deltaTime));
    }
}
