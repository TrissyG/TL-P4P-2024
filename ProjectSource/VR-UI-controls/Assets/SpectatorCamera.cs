using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpectatorCamera : MonoBehaviour
{
    /*
     * The spectator camera moves between a fixed perspective (its starting
     * position), a locationing mode perspective (only in the Blank World, ) and a first person perspective 
     * (matching the perspective of the VR display).
     */
    enum CameraState
    {
        fixedPerspective,
        locationingPerspective,
        firstPersonPerspective
    }

    private CameraState state = CameraState.fixedPerspective;
    private Vector3 fixedPerspectivePosition;
    private Quaternion fixedPerspectiveRotation;
    private Vector3 locationingPerspectivePosition;
    private Quaternion locationingPerspectiveRotation;
    
    private float defaultPerspectiveFieldOfView = 60f;
    private float locationingPerspectiveFieldOfView = 130f;
    
    public GameObject VRCamera;

    // Start is called before the first frame update
    void Start()
    {
        // Get a reference to the VR camera
        VRCamera = GameObject.Find("Main Camera");
        Camera mainCamera = VRCamera.GetComponent<Camera>();
        fixedPerspectivePosition = gameObject.transform.position;
        fixedPerspectiveRotation = gameObject.transform.rotation;
        if (mainCamera != null)
        {
            mainCamera.depthTextureMode = DepthTextureMode.Depth;
        }
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

    public void ToLocationingPerspective()
    {
        if (state == CameraState.locationingPerspective) return;// do nothing if in wrong state

        if (SceneManager.GetActiveScene().name == "BlankWorld")
        {
            // Set the locationing perspective position and rotation
            locationingPerspectivePosition = new Vector3(-0.36f, 3.44f, -1.5f);
            locationingPerspectiveRotation = Quaternion.Euler(160f, 0f, 180f);

            gameObject.transform.position = locationingPerspectivePosition;
            gameObject.transform.rotation = locationingPerspectiveRotation;
            gameObject.GetComponent<Camera>().fieldOfView = locationingPerspectiveFieldOfView;
        }
        else {
            Debug.Log("Locationing Perspective only available in the Blank World scene.");
        }
    }

    public void ToFirstPersonPerspective()
    {
        if (state == CameraState.firstPersonPerspective) return;// do nothing if in wrong state

        // Save the fixed perspective position and rotation as the camera's current position
        fixedPerspectivePosition = gameObject.transform.position;
        fixedPerspectiveRotation = gameObject.transform.rotation;
        gameObject.GetComponent<Camera>().fieldOfView = defaultPerspectiveFieldOfView;

        // Start tracking the VR camera
        state = CameraState.firstPersonPerspective;
    }
}
