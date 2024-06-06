using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositioningAssistGhost : MonoBehaviour
{

    public Transform playerHead;

    public Mesh mesh;

    public GameObject ghost;
    public GameObject stick;

    public float objectOutOfViewAngle = 45;
    public float ghostDefaultDistance = 1.0f;
    public float headPivotOffset = 0.0f;

    private Material material;
    private RenderParams rp;

    private Vector3 vector;

    private BoxCollider ghostCollider;


    private bool isGhostGrabbed = false;

    private Vector3 playerPivotPoint;

    // Start is called before the first frame update
    void Start()
    {
        //mesh = gameObject.GetComponent<MeshFilter>().mesh;
        material = gameObject.GetComponent<Renderer>().material;
        rp = new RenderParams(material);

        //stick = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        //Destroy(stick.GetComponent<Collider>());
        stick.transform.localScale = new Vector3(0.05f, 1, 0.05f);

    }

    // Update is called once per frame
    void Update()
    {
        playerPivotPoint = playerHead.position;
        playerPivotPoint += Vector3.up * headPivotOffset;


        // for gizmo
        vector = playerHead.position - ghost.transform.position;
        vector *= 2;

        // ideas
        // Ignore whether the ghost is grabbed. The only important factors are the FOV and headlock.

        // NEVER show the ghost when not headlocked.
        if (gameObject.GetComponent<ParentSetter>().reparenting == true) {
            // object is headlocked

            if (isGhostGrabbed) {
                // place the real object at the other end of the stick (while grabbing ghost)
                gameObject.transform.position = ghost.transform.position + ((playerPivotPoint - ghost.transform.position) * 2);


            } else {
                // Ghost is not grabbed
                if (Vector3.Angle(playerHead.transform.forward, -(playerPivotPoint - transform.position)) > objectOutOfViewAngle) {
                    // Radio is outside of player's field of view

                    ghost.SetActive(true);
                    stick.SetActive(true);

                    // Draw radio directly in front of player
                    //Graphics.RenderMesh(rp, mesh, 0, Matrix4x4.TRS(playerHead.transform.position + (playerHead.transform.forward * ghostDefaultDistance), ghostRotation, Vector3.one));
                } else {
                    // Radio is inside the player's field of view
                    ghost.SetActive(false);
                    stick.SetActive(false);
                }

                // object is not headlocked
                //if (gameObject.GetComponent<ParentSetter>().parent == null) {
                    // place the ghost directly in the player's view
                //    ghost.transform.position = playerPivotPoint + (playerHead.transform.forward * ghostDefaultDistance);
                //} else {
                    // ghost tracks the real object (across stick)
                    ghost.transform.position = gameObject.transform.position + ((playerPivotPoint - gameObject.transform.position) * 2);
                //}

            }
        } else {
            // object is not headlocked
            ghost.SetActive(false);
            stick.SetActive(false);
        }

        

        

        // Have the stick connect the ghost and real object
        stick.transform.position = ghost.transform.position + ((gameObject.transform.position - ghost.transform.position) / 2);
        stick.transform.localScale = new Vector3(0.15f, (gameObject.transform.position - ghost.transform.position).magnitude / 2, 0.15f);
        stick.transform.rotation = Quaternion.FromToRotation(Vector3.up, (gameObject.transform.position - ghost.transform.position).normalized);
    }

    public void GhostGrabbed()
    {
        isGhostGrabbed = true;
    }

    public void GhostLetGo()
    {
        isGhostGrabbed = false;
    }

    private void OnDisable()
    {
        ghost.SetActive(false);
        stick.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + vector);
        Gizmos.DrawLine(stick.transform.position, stick.transform.position + stick.transform.up);
        Gizmos.DrawLine(stick.transform.position, stick.transform.position + stick.transform.forward);
    }
}
