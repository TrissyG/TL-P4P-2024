using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimingDistanceManager : MonoBehaviour
{
private OVRPointerVisualizer pointerVisualizer;
    public GameObject targetObject;

    void Start()
    {
        // // Get the OVRPointerVisualizer component
        // pointerVisualizer = GetComponent<OVRPointerVisualizer>();
        // Debug.Log("Pointer Visualizer: " + pointerVisualizer);
        // if (pointerVisualizer == null)
        // {
        //     Debug.LogError("OVRPointerVisualizer component not found on this GameObject.");
        // }
    }

    void Update()
    {
        // if (pointerVisualizer != null && targetObject != null)
        // {
        //     // Check if the A button is pressed
        //     if (OVRInput.GetDown(OVRInput.Button.One))
        //     {
        //         MeasureDistanceToTarget();
        //     }
        // }
    }

    private void MeasureDistanceToTarget()
    {
        Ray ray = new Ray(pointerVisualizer.rayTransform.position, pointerVisualizer.rayTransform.forward);
        Vector3 closestPoint = GetClosestPointOnRay(ray, targetObject.transform.position);
        float distance = Vector3.Distance(closestPoint, targetObject.transform.position);
        Debug.Log("Distance to target object: " + distance);
    }

    private Vector3 GetClosestPointOnRay(Ray ray, Vector3 point)
    {
        Vector3 rayToPoint = point - ray.origin;
        float projectionLength = Vector3.Dot(rayToPoint, ray.direction);
        return ray.origin + ray.direction * projectionLength;
    }
}
