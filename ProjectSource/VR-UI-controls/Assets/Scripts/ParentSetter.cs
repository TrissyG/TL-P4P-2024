using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentSetter : MonoBehaviour
{
    
    public GameObject parent = null;
    public GameObject child;
    public Rigidbody rb;
    public bool reparenting = false;

    /*
     * called on the object in XR Grab Interactable, in Interactable Events
     * checks to see if parenting has been aproved once the user releases/unselects the object
    */
    public void SetParentToCamera(){
        // Activated when the object is released

        if (parent != null) {
            // ensure headlocked state is always saved when headlocked
            Settings.Instance.SetValue(gameObject.name + "IsHeadlocked", true);
            Settings.Instance.SetValue(gameObject.name + "HeadlockedPosition", child.transform.localPosition);
            Settings.Instance.SetValue(gameObject.name + "HeadlockedRotation", child.transform.localEulerAngles);
            Settings.Instance.SetValue("lastHeadlockedObject", gameObject.name);
        }

        if (parent == null && reparenting) 
        {
            parent = GameObject.Find("XR Origin/Camera Offset/Main Camera");
            child.transform.SetParent(parent.transform, true);
            rb.useGravity = false;
            rb.isKinematic = true;
            //Debug.Log("setting parent to camera");
            //Debug.Log("Gravity : " + rb.useGravity + " Kinematics : " + rb.isKinematic);
            Settings.Instance.SetValue(gameObject.name + "IsHeadlocked", true);
            Settings.Instance.SetValue(gameObject.name + "HeadlockedPosition", child.transform.localPosition);
            Settings.Instance.SetValue(gameObject.name + "HeadlockedRotation", child.transform.localEulerAngles);
            Settings.Instance.SetValue("lastHeadlockedObject", gameObject.name);
        } 
        else if (!reparenting)
        {
            parent = null;
            child.transform.SetParent(null);
            rb.useGravity = true;
            rb.isKinematic = false;
            //Debug.Log("setting parent to null");
            //Debug.Log("Gravity : " + rb.useGravity + " Kinematics : " + rb.isKinematic);
            Settings.Instance.SetValue(gameObject.name + "IsHeadlocked", false);
        }
    }

    public void ToggleReparenting() 
    {
        // Activated by trigger press
        if (reparenting == false) {
            reparenting = true;
        } else {
            reparenting = false;
        }
    }

    // This method may be overridden for objects with specific reset behaviour.
    public virtual void ResetReparenting()
    {
        reparenting = false;
        SetParentToCamera();
        if (gameObject.GetComponent<HighlightRadio>() != null) gameObject.GetComponent<HighlightRadio>().SetUnlock();
    }
    protected virtual void ForceEnterHeadlocking()
    {
        gameObject.GetComponent<HighlightRadio>().ToggleHighlight();
    }

    public void TryRestoreLastHeadlock()
    {
        // Ensure data exists before proceeding
        if (!Settings.Instance.Contains(gameObject.name + "HeadlockedPosition") || !Settings.Instance.Contains(gameObject.name + "HeadlockedPosition")) return;

        // Retrieve position and rotation
        Vector3 previousPosition;
        Settings.Instance.GetValue(gameObject.name + "HeadlockedPosition", out previousPosition);
        Vector3 previousRotation;
        Settings.Instance.GetValue(gameObject.name + "HeadlockedRotation", out previousRotation);

        // Restore headlocked state
        ToggleReparenting();
        SetParentToCamera();
        ForceEnterHeadlocking();

        // Restore position and rotation
        child.transform.localPosition = previousPosition;
        child.transform.localEulerAngles = previousRotation;

        // Save position and rotation
        Settings.Instance.SetValue(gameObject.name + "HeadlockedPosition", child.transform.localPosition);
        Settings.Instance.SetValue(gameObject.name + "HeadlockedRotation", child.transform.localEulerAngles);
    }

    private void Start()
    {
        Settings.Instance.BlacklistFromSaving(gameObject.name + "IsHeadlocked");

        // Try and restore the headlock from previous scene
        if (Settings.Instance.Contains(gameObject.name + "IsHeadlocked")) {
            Settings.Instance.GetValue(gameObject.name + "IsHeadlocked", out bool isHeadlocked);
            if (isHeadlocked) {
                TryRestoreLastHeadlock();
            }
        }
    }

}
