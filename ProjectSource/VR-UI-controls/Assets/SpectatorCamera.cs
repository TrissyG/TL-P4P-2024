using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectatorCamera : MonoBehaviour
{
    /*
     * The spectator camera moves between a fixed perspective (its starting
     * position) and a first person perspective (matching the perspective of 
     * the VR display).
     */
    enum CameraState
    {
        fixedPerspective,
        firstPersonPerspective
    }

    private CameraState state = CameraState.fixedPerspective;
    private Vector3 fixedPerspectivePosition;
    private Quaternion fixedPerspectiveRotation;
    public GameObject VRCamera;

    // Start is called before the first frame update
    void Start()
    {
        // Get a reference to the VR camera
        VRCamera = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (state == CameraState.firstPersonPerspective) {
            // track the VR camera's position and rotation
            gameObject.transform.position = VRCamera.transform.position;
            gameObject.transform.rotation = VRCamera.transform.rotation;
        }
    }

    public void ToFixedPerspective()
    {
        if (state == CameraState.fixedPerspective) return;// do nothing if in wrong state

        // Stop tracking the VR camera
        state = CameraState.fixedPerspective;

        // Restore the camera to the fixed position/rotation
        gameObject.transform.position = fixedPerspectivePosition;
        gameObject.transform.rotation = fixedPerspectiveRotation;
    }

    public void ToFirstPersonPerspective()
    {
        if (state == CameraState.firstPersonPerspective) return;// do nothing if in wrong state

        // Save the fixed perspective position and rotation as the camera's current position
        fixedPerspectivePosition = gameObject.transform.position;
        fixedPerspectiveRotation = gameObject.transform.rotation;

        // Start tracking the VR camera
        state = CameraState.firstPersonPerspective;
    }
}
