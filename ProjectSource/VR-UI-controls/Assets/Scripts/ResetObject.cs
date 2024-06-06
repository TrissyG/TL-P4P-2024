using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//references: https://answers.unity.com/questions/1359710/i-want-to-reset-my-object-to-its-original-position.html
// https://forum.unity.com/threads/oculus-quest-how-to-detect-a-b-x-y-button-presses.1108232/
// https://forum.unity.com/threads/inputactionreference-enabled-but-nothing-occurs.1086995/

public class ResetObject : MonoBehaviour
{

    private Vector3 startingPosition;
    private Quaternion startingRotation;

    private struct Child
    {
        public Rigidbody rigidbody;
        public Vector3 startingPosition;
        public Quaternion startingRotation;

        public Child(Rigidbody rigidbody, Vector3 startingPosition, Quaternion startingRotation)
        {
            this.rigidbody = rigidbody;
            this.startingPosition = startingPosition;
            this.startingRotation = startingRotation;
        }
    }

    private List<Child> children;

    
    public new GameObject gameObject;
    public InputActionReference inputActionReferencePrimaryLeft;
    [Tooltip("Uses the component attached to the Game Object if not set here")]
    public ParentSetter parentSetter;
    // Start is called before the first frame update
    void Start()
    {
        startingPosition = gameObject.transform.position;
        startingRotation = gameObject.transform.rotation;

        // also store staring position/rotation for any children. this is appliccable to chime bells.
        children = new List<Child>();
        foreach (Rigidbody rigidbody in gameObject.transform.root.GetComponentsInChildren<Rigidbody>()) {
            if (rigidbody != gameObject.GetComponent<Rigidbody>()) {
                children.Add(new Child(rigidbody, rigidbody.transform.position, rigidbody.transform.rotation));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        inputActionReferencePrimaryLeft.action.performed += c => resetObject();
    }

    private void resetObject() {
        // reset position, rotation, and velocity
        gameObject.transform.position = startingPosition;
        gameObject.transform.rotation = startingRotation;
        Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;

        // also reset children if applicabble, prevent chime bells from freaking out
        foreach (Child child in children) {
            child.rigidbody.transform.position = child.startingPosition;
            child.rigidbody.transform.rotation = child.startingRotation;
            child.rigidbody.velocity = Vector3.zero;
            child.rigidbody.angularVelocity = Vector3.zero;
        }
        if (parentSetter == null) {
            gameObject.GetComponent<ParentSetter>().ResetReparenting();
        } else {
            parentSetter.ResetReparenting();
        }
    }
}
