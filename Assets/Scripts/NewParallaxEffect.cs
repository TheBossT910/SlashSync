using UnityEngine;

public class NewParallaxEffect: MonoBehaviour {

    //we can set the camera and the parallaxFactor
    public Camera cam;
    //parallaxFactor = 1 means it goes the same speed as the camera (relatively, it becomes still)
    public Vector3 parallaxFactor;
    private Transform cameraTransform;
    private Vector3 lastCameraPosition;

    private void Start() {
        cameraTransform = cam.transform;
        lastCameraPosition = cameraTransform.position;
    }

    private void FixedUpdate() {
        //find how much the camera has moved
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        //moves the object attached with this script
        transform.position += new Vector3(deltaMovement.x * parallaxFactor.x, deltaMovement.y * parallaxFactor.y, transform.position.z);
        lastCameraPosition = cameraTransform.position;

    }
}