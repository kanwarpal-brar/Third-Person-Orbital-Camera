using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbitScript : MonoBehaviour
{
    public GameObject target;  // This the the object that the camera is orbiting
    public float distance = 5.0f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;
 
    public float yMinLimit = 20f;
    public float yMaxLimit = 80f;
 
    public float distanceMin = 5f;
    public float distanceMax = 15f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
