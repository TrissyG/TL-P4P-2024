using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpdateRatingValue : MonoBehaviour
{
    private TextMeshProUGUI tmp;
    private TextMeshProUGUI descriptionText;
    private Slider slider;

    private string[] descriptionValues = new string[]{"Strongly Disagree", "Disagree", "Neutral", "Agree", "Strongly Agree"};
    // Start is called before the first frame update
    void Start()
    {
        // Get the root transform (rating tablet)
        Transform ratingTabletRoot = gameObject.transform.root;

        // Get references to child components
        tmp = ratingTabletRoot.Find("RatingCanvas/RatingValue").GetComponent<TextMeshProUGUI>();
        //tmp = GameObject.Find("RatingValue").GetComponent<TextMeshProUGUI>();
        //descriptionText = ratingTabletRoot.Find("RatingCanvas/RatingValueDescription").GetComponent<TextMeshProUGUI>();
        //descriptionText = GameObject.Find("RatingValueDescription").GetComponent<TextMeshProUGUI>();
        slider = ratingTabletRoot.Find("RatingCanvas/RatingSlider").GetComponent<Slider>();
        //slider = GameObject.Find("RatingSlider").GetComponent<Slider>();

        
    }

    // Update is called once per frame
    void Update()
    {
        tmp.text = ((int)slider.value).ToString();
        // description text is disabled
        //descriptionText.text = descriptionValues[(int)slider.value - 1];
    }

    
}
