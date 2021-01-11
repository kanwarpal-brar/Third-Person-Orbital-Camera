using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalCam : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;  // This is the designated have object the camera must orbit
    public float xSpeedMax = 15f;
    public float xSpeedMin = 0f;
    public float xSpeed = 0f;  // Starting speed is 1
    public float ySpeedMax = 15f;
    public float ySpeedMin = 0;
    public float ySpeed = 0f;
    public float zoomDistance = 5f;
    public float minDistance = 3f;
    public float maxDistance = 10f;
    public float yMax = 90f;
    public float yMin = -8f;
    public float yPaddingBase = 8f;
    public float cameraWeight = 5f;  // This affects the speed of the camera through acceleration
    private static float xAngle;
    private static float yAngle;
    private float distToGround;
    private bool isGrounded;
    void Start()
    {
        // Set the initial position
    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        // The below operations prevent the camera from clipping the ground
        isOnGround();
        distanceToGround();
        yMin = yPaddingBase - distToGround;
    }
    void LateUpdate()  // Lateupdate is used to all objects move before the camera
    {
        // When position transformations are applied instead of interpolated, you must manually increment the transformation value for interpolation

        // zoomDistance = Mathf.Clamp(zoomDistance - 5*Input.GetAxis("Mouse ScrollWheel"), minDistance, maxDistance);  // Makes sure the value of zoom is withtin valid range
        // rotationX = Quaternion.Euler(0,Input.GetAxis("Mouse X"),0);
        // // Below transformation creates a rotation based vector with a specific distance from the target, then adds the target position to focus it.
        // position = rotationX * new Vector3(0,0,-zoomDistance) + target.position;
        // transform.RotateAround(target.position, rotationX.eulerAngles, xSpeed*Time.deltaTime);
        // transform.position = position;

        // If the mouse axis is not being moved, set the xSpeed to minimum

        // Retrieve adjusted zoomDistance by mousewheel axis
        zoomDistance = Mathf.Clamp(zoomDistance - 2*Input.GetAxis("Mouse ScrollWheel"), minDistance, maxDistance);  // 5 is an arbitrary step value (zoom sensitivity)
        // Construct the increment in current turning angle in x, by Mouse X axis; same with y
        xAngle += Input.GetAxis("Mouse X") * xSpeed;
        yAngle = Mathf.Clamp(yAngle+Input.GetAxis("Mouse Y") * ySpeed, yMin, yMax);  // The y angle is clamped to a set range
        // Accelerate the camera speed
        if (Input.GetAxis("Mouse X") == 0 ) {
            xSpeed = xSpeedMin;
        } else {
            xSpeed = ApplyAcceleration(xSpeed, cameraWeight, xSpeedMin, xSpeedMax, Time.deltaTime);
        }
        if (Input.GetAxis("Mouse Y") == 0) {
            ySpeed = ySpeedMin;
        } else {
            ySpeed = ApplyAcceleration(ySpeed, cameraWeight, ySpeedMin, ySpeedMax, Time.deltaTime);
        }
        // Construct the rotation of the camera around the target through the Mouse X axis
        Quaternion rotationX = Quaternion.Euler(yAngle, xAngle, transform.rotation.z);
        // Setup the camera position based off of current rotation angle and target location, multiply rotation by zoom distance to move the camera in negative
        // Z space, away from target. Add the target location to this to offset to correct location
        // Multiplying a vector by a quartonian applies a rotatio  to it
        Vector3 position = rotationX * new Vector3(0,0,-zoomDistance) + target.position;

        // Apply all transformations
        transform.rotation = rotationX;
        transform.position = position;
    }
    private float ApplyAcceleration(float initialSpeed, float acceleration, float min, float max, float time) {  // Accelerates a speed within a clamped range
    // Acceleration must be multiplied by a second difference
        return Mathf.Clamp(initialSpeed + acceleration*time, min, max);  // Returns the clamped speed after acceleration. This gives the camera movement more weight
    }

    private void distanceToGround()
    {
        RaycastHit hit;
        Vector3 dir = new Vector3(0, -1);
        if ((Physics.Raycast(transform.position, dir, out hit, 100000f)) && isGrounded)
        {
            distToGround = hit.distance;
        }
        else
        {
            distToGround = 0;
        }

    }
    private void isOnGround()
    {  // This function raycasts the ground and returns true or false depending on whether or not the script object is touching any object labelled ground
        RaycastHit hit;
        float distance = 1f;
        Vector3 dir = new Vector3(0, -1);
        if (Physics.Raycast(transform.position, dir, out hit, distance))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
            distanceToGround();
        }
    }
}
