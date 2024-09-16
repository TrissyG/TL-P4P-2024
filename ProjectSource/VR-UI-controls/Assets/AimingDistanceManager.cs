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
    public GameObject inputActionManager;
    public InputDevice rightHandDevice;




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

        // Get the right-hand controller's InputDevice
        // Get the right-hand controller's InputDevice
        // List<InputDevice> devices = new List<InputDevice>();
        // InputDevices.GetDevicesAtXRNode(XRNode.RightHand, devices);
        // if (devices.Count > 0)
        // {
        //     rightHandDevice = devices[0];
        //     Debug.Log("Right hand InputDevice found.");
        // }
        // else
        // {
        //     Debug.LogError("Right hand InputDevice not found.");
        // }
        // UnityEngine.XR.InputDevice device = UnityEngine.XR.InputDevices.GetDeviceAtXRNode(UnityEngine.XR.XRNode.RightHand);
        // if (device.isValid)
        // {
        //     Debug.Log("Device = " + device.name);
        //     Debug.Log("Device is valid.");
        // }
        // else
        // {
        //     Debug.Log("Device = " + device.name);
        //     Debug.Log("Device is not valid.");
        // }

        // rightHandXRController = rightHandController.GetComponent<XRController>();
        // if (rightHandXRController == null)
        // {
        //     Debug.LogError("XRController component not found on the right-hand controller.");
        // }

    }

    void Update()
    {
        if (rightHandRayInteractor != null && targetObject != null && pointablePlane != null)
        {
            // If the XRI Right Hand Interactor is active and the 'A' button is pressed, measure the distance to the target object

            // if (rightHandController.XRController.inputDevice.IsPressed(inputActionManager.GetComponent<UnityEngine.XR.Interaction.Toolkit.Inputs.Simulation.XRSimulatedControllerInputDeviceState>().selectAction.action))
            // {
            //     Debug.Log("'A' button pressed on the right-handed controller.");
            //     MeasureDistanceToTarget();
            // }
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
