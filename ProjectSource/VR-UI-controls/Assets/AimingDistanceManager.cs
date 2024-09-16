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
    public PointablePlane pointablePlane;
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor rightHandRayInteractor;
    // public GameObject inputActionManager;
    private InputData _inputData;

    // Cooldown duration in seconds
    public float cooldownDuration = 2.0f;
    // Time when MeasureDistanceToTarget was last called
    private float lastCallTime = 0.0f;


    private Vector3 planeNormal;
    private Vector3 planePoint;



    void Start()
    {
        // Get the PointablePlane component from the target object
        if (targetObject != null)
        {
            pointablePlane = targetObject.GetComponent<PointablePlane>();
            if (pointablePlane == null)
            {
                Debug.LogError("PointablePlane component not found on the target object.");
            }

        }

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
    }

    void Update()
    {
        if (rightHandRayInteractor != null && targetObject != null && pointablePlane != null)
        {
            if (_inputData._rightController.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue) && primaryButtonValue)
            {
                // Check if the cooldown period has passed
                if (Time.time >= lastCallTime + cooldownDuration)
                {
                    Debug.Log("'A' button pressed on the right-handed controller.");
                    MeasureDistanceToTarget();
                    // Update the last call time
                    lastCallTime = Time.time;
                }
            }
        }
    }

    private void MeasureDistanceToTarget()
    {
        // Ray ray;
        // if (rightHandRayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        // {
        //     ray = new Ray(hit.point, hit.normal);
        //     if (pointablePlane.Raycast(ray, out SurfaceHit surfaceHit, Mathf.Infinity))
        //     {
        //         float distance = Vector3.Distance(surfaceHit.Point, targetObject.transform.position);
        //         Debug.Log("Distance to target object: " + distance);
        //     }
        //     else
        //     {
        //         Debug.Log("Raycast did not hit the PointablePlane.");
        //     }
        // }
        // else
        // {
        //     Debug.Log("Raycast did not hit any object.");
        // }

        // Create a ray from the controller or camera's position and forward direction
        Ray ray = new Ray(rightHandController.transform.position, rightHandController.transform.forward);

        // Get the target object's position
        Vector3 targetPosition = targetObject.transform.position;
        Vector3 rayOrigin = rightHandController.transform.position;

        // Calculate the direction from the user to the target object
        Vector3 directionToTarget = (targetPosition - rayOrigin).normalized;

        // Define the plane's normal as the direction to the target
        Vector3 planeNormal = directionToTarget;

        // Calculate the intersection point of the ray with the plane
        Plane plane = new Plane(planeNormal, targetPosition);
        if (plane.Raycast(ray, out float enter))
        {
            // Find the point where the ray intersects the plane
            Vector3 intersectPoint = ray.GetPoint(enter);

            // Get the YZ components of the intersection point and target position
            Vector2 intersectPointYZ = new Vector2(intersectPoint.y, intersectPoint.z);
            Vector2 targetPositionYZ = new Vector2(targetPosition.y, targetPosition.z);

            // Calculate the horizontal distance between the intersection point and the target object on the YZ plane
            float distanceYZ = Vector2.Distance(intersectPointYZ, targetPositionYZ);

            // Log the distance for debugging
            Debug.Log("Distance from aim point to target object (YZ): " + distanceYZ);
        }
        else
        {
            Debug.Log("Ray did not intersect the plane.");
        }
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
