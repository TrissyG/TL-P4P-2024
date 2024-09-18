using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction.Surfaces;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class AimingDistanceManager : MonoBehaviour
{
    public GameObject rightHandController;
    public GameObject leftHandController;
    public GameObject targetObject;
    // public PointablePlane pointablePlane;
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor rightHandRayInteractor;
    // public GameObject inputActionManager;
    private InputData _inputData;
    private DataLoggingManager _dataLoggingManager;
    // Cooldown duration in seconds for the MeasureDistanceToTarget method
    private float cooldownDuration = 2.0f;
    // Time when MeasureDistanceToTarget was last called
    private float lastCallTime = 0.0f;

    // Plane properties for debugging
    private Vector3 planeNormal;
    private Vector3 planePoint;

    void Start()
    {
        // Find the right-hand controller's XRRayInteractor
        rightHandRayInteractor = FindObjectOfType<UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor>();
        if (rightHandRayInteractor == null)
        {
            Debug.LogError("XRRayInteractor component not found for the right-hand controller.");
        }

        // Initialize _inputData
        _inputData = FindObjectOfType<InputData>();
        if (_inputData == null)
        {
            Debug.LogError("InputData component not found in the scene.");
        }
        _dataLoggingManager = FindObjectOfType<DataLoggingManager>();
        if (_dataLoggingManager == null)
        {
            Debug.LogError("DataLoggingManager component not found in the scene.");
        }
    }

    void Update()
    {
        if (rightHandRayInteractor != null && targetObject != null)
        {
            if (_inputData._rightController.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue) && primaryButtonValue)
            {
                // Check if the cooldown period has passed
                if (Time.time >= lastCallTime + cooldownDuration)
                {
                    Debug.Log("'A' button pressed on the right-handed controller.");
                    MeasureDisplacementAngle();
                    // Update the last call time
                    lastCallTime = Time.time;
                    _dataLoggingManager.pressLocationingButton();
                }
            }
        }
    }

    private void MeasureDisplacementAngle()
    {
        // Create a ray from the controller's position in the forward direction to cast towards the target object
        Ray ray = new Ray(rightHandController.transform.position, rightHandController.transform.forward);

        Vector3 targetPosition = targetObject.transform.position;
        Vector3 rayOrigin = rightHandController.transform.position;

        // Calculate the direction from the user to the target object
        Vector3 directionToTarget = (targetPosition - rayOrigin).normalized;

        // Calculate the displacement angle between the user's aim and the vector to the target object
        float displacementAngle = Vector3.Angle(ray.direction, directionToTarget);

        // Log the displacement angle for debugging
        Debug.Log("Displacement angle between aim and target object: " + displacementAngle);

        // Update DataLoggingManager with the calculated values
        _dataLoggingManager.setDisplacementAngle(displacementAngle);
        _dataLoggingManager.setAudioSourceLocation(targetPosition);
        _dataLoggingManager.setRayOriginPoint(rayOrigin);
        _dataLoggingManager.setRayIntersectPoint(targetPosition); // Use target position as the intersect point
        _dataLoggingManager.setAudioSourcePlaneNormal(new Vector2(directionToTarget.y, directionToTarget.z)); // Use the y and z components of the direction vector as the plane normal
    }

    private void OnDrawGizmos()
    {
        if (planeNormal != Vector3.zero)
        {
            // Draw the plane
            Gizmos.color = Color.green;
            Vector3 planeCenter = planePoint;
            Vector3 planeSize = new Vector3(1, 1, 0.01f); // Adjust the size as needed

            // Calculate the plane's orientation
            Quaternion planeRotation = Quaternion.LookRotation(planeNormal);

            // Draw the plane as a cube
            Gizmos.matrix = Matrix4x4.TRS(planeCenter, planeRotation, planeSize);
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        }
    }
}
