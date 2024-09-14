using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction.Surfaces;
using UnityEngine;

public class AimingDistanceManager : MonoBehaviour
{
    public GameObject rightHandController;
    public GameObject leftHandController;
    public GameObject targetObject;
    public PointablePlane pointablePlane;
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor rightHandRayInteractor;


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
            Debug.Log("PointablePlane component found on the target object.");
        }

        // Find the right-hand controller's XRRayInteractor
        rightHandRayInteractor = FindObjectOfType<UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor>();
        if (rightHandRayInteractor == null)
        {
            Debug.LogError("XRRayInteractor component not found for the right-hand controller.");
        }
        Debug.Log("XRRayInteractor component found for the right-hand controller.");
        // show which parent object the ray interactor is attached to
        Debug.Log("Right hand ray interactor parent object: " + rightHandRayInteractor.transform.parent.gameObject.name);
        // show which object the ray interactor is attached to
        Debug.Log("Right hand ray interactor object: " + rightHandRayInteractor.gameObject.name);
        
    }

    void Update()
    {
        if (rightHandRayInteractor != null && targetObject != null && pointablePlane != null)
        {   
            // Check if the A button is pressed on the right-handed controller
            if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
            {
                MeasureDistanceToTarget();
            }
        }
    }

    private void MeasureDistanceToTarget()
    {
        Ray ray;
        if (rightHandRayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            ray = new Ray(hit.point, hit.normal);
            if (pointablePlane.Raycast(ray, out SurfaceHit surfaceHit, Mathf.Infinity))
            {
                float distance = Vector3.Distance(surfaceHit.Point, targetObject.transform.position);
                Debug.Log("Distance to target object: " + distance);
            }
            else
            {
                Debug.Log("Raycast did not hit the PointablePlane.");
            }
        }
        else
        {
            Debug.Log("Raycast did not hit any object.");
        }
    }
}
