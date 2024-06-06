using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ChimeAntigravity : MonoBehaviour
{
    private List<Rigidbody> bells;
    private Rigidbody bellsBase;

    [Tooltip("Allow bells to collide with objects other than the chimes.")]
    public bool allowCollisions = false;

    [Tooltip("Prevents bell physics from reacting to the base moving.")]
    public bool parentToBase = true;

    // Start is called before the first frame update
    void Start()
    {
        // Get references to bells and base (which bells hang from)
        bells = new List<Rigidbody>();
        foreach (GameObject bell in this.GetComponent<ChimeConfiguration>().bells) {
            bells.Add(bell.GetComponent<Rigidbody>());
        }
        bellsBase = this.GetComponent<ChimeConfiguration>().bellsBase.GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        if (bells == null) Start();

        foreach (Rigidbody bell in bells) {
            // Disable gravity on the bells
            bell.useGravity = false;

            if (!allowCollisions) {
                bell.gameObject.layer = LayerMask.NameToLayer("CollisionMask");
            }
            if (parentToBase) {
                bell.transform.parent = bellsBase.transform;
            }
        }
        // Disable physics on the bells base
        bellsBase.isKinematic = true;
    }

    private void OnDisable()
    {
        if (!gameObject.activeInHierarchy) {
            // don't bother with disable behaviour if the entire gameObject is being disabled/destroyed
            return;
        }

        foreach (Rigidbody bell in bells) {
            // Enable gravity on the bells
            bell.useGravity = true;

            bell.gameObject.layer = LayerMask.NameToLayer("Default");
            bell.transform.parent = this.transform;
        }
        // Enable physics on the bells base (but don't interfere if being controlled by XR)
        if (bellsBase.GetComponent<XRGrabInteractable>().isSelected == false) {
            bellsBase.isKinematic = false;
        }
        
    }

    private void FixedUpdate()
    {
        // Apply gravity in the relative direction of the bells base
        foreach (Rigidbody bell in bells) {
            bell.AddForce(bellsBase.transform.up * -1 * Physics.gravity.magnitude, ForceMode.Acceleration);
        }
    }

    public void ToggleEnabled()
    {
        if (enabled) {
            enabled = false;
        } else {
            enabled = true;
        }
    }
}
