using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatingTabletController : MonoBehaviour
{
    private int pageIndex;
    private string[] questions = new string[]{
        "I enjoyed this scenario",
        "This scenario felt very realistic",
        "I think the sound emitted by the radio was believable",
        "When moving the radio, I noticed the sound would move with it",
        "I felt very relaxed during this scenario",
        "Being able to interact with objects increased the realism of the environment"
        };
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("fdsa");
    }
}
