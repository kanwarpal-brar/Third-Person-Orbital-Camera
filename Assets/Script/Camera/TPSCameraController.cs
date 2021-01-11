using ProBuilder2.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class TPSCameraController : MonoBehaviour
{
    public Transform target;
    public float xSpeedMax = 15f;
    public float xSpeedMin = 0f;
    public float xSpeed = 1f;
    public float ySpeedMax = 15;
    public float ySpeedMin = 0f;
    public float ySpeed = 1f;
    public float zoomDistance = 5f;
    public float minDistance = 3f;
    public float maxDistance = 10f;
    public float yMax = 90f;
    public float yMin = -10f;
    public float cameraWeight = 5f;
    private float xAngle;
    private float yAngle;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;  // Hides the mouse course
        target = transform.parent;  // returns the parent object, which would be the target
        transform.position = target.position + (-target.forward * 2f);  // Sets the starting distance from character
        transform.LookAt(target);  // Sets the camera to start looking at the target
    }

    // Update is called once per frame
    void LateUpdate()  // Let update is used so all camera operations occur at the end of other operations
    {
        cameraControl();
    }
    private void cameraControl()
    {
        // Calculate what the current speed should be with camera acceleration
        if (Input.GetAxis("Mouse X") == 0)
        {
            xSpeed = xSpeedMin;
        }
        else
        {
            xSpeed = ApplyAcceleration(xSpeed, cameraWeight, xSpeedMin, xSpeedMax, Time.deltaTime);
        }
        if (Input.GetAxis("Mouse Y") == 0)
        {
            ySpeed = ySpeedMin;
        }
        else
        {
            ySpeed = ApplyAcceleration(ySpeed, cameraWeight, ySpeedMin, ySpeedMax, Time.deltaTime);
        }
        // Calculate the current angle of the camera by mouse movement
        xAngle += Input.GetAxis("Mouse X") * xSpeed;
        yAngle -= Input.GetAxis("Mouse Y") * ySpeed;
        yAngle = Mathf.Clamp(yAngle, yMin, yMax);  // Clamps the y angle to a specific range
        transform.LookAt(target);  // Transforms the camera to look at the target object

        // Apply rotation to the target object (which is invisibily attached to the player)
        target.rotation = Quaternion.Euler(yAngle, xAngle, 0);

        // Below operations translate camera position to affect zoom
        transform.position = transform.position + (new Vector3(0, 0, 1) * Input.GetAxis("Mouse ScrollWheel"));

    }
    private float ApplyAcceleration(float initialSpeed, float acceleration, float min, float max, float time)
    {  // Accelerates a speed within a clamped range
       // Acceleration must be multiplied by a second difference
        return Mathf.Clamp(initialSpeed + acceleration * time, min, max);  // Returns the clamped speed after acceleration. This gives the camera movement more weight
    }
}
