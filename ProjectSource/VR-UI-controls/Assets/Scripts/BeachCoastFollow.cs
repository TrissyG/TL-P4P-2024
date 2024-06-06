using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeachCoastFollow : MonoBehaviour
{
    public GameObject Player;
    public GameObject PosZPoint;
    public GameObject NegZPoint;

    private Vector3 playerDistance;
    private Vector3 pathDirection;
    private Vector3 closePosition;

    // Start is called before the first frame update
    void Start()
    {
        // Vector between two points along the coast that create a "path" for the audio object to follow
        pathDirection = NegZPoint.transform.position - PosZPoint.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Uses the coast "path" as a "rail" for the audio object to follow ensuring it is always as close to the player as it can be while staying on the "path"
        playerDistance = Player.transform.position - transform.position;
        pathDirection = pathDirection.normalized;
        closePosition = Vector3.Project(playerDistance, pathDirection);
        transform.Translate(closePosition);
        //Debug.Log(closePosition);

        //Debug.DrawRay(transform.position, playerDistance);
    }
}
