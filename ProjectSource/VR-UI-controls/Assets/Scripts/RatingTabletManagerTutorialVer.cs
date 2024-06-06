using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;

public class RatingTabletManagerTutorialVer : MonoBehaviour
{
    private TextMeshProUGUI questionText;
    private TextMeshProUGUI pageIndexText;
    private TextMeshProUGUI nextButtonText;
    private GameObject backButton;
    private Slider slider;
    private GameObject mainCanvas;
    private GameObject completeCanvas;

    private int pageIndex = 0;
    private string[] questions = new string[]{
        "I can interact with the tablet using the trigger on the right controller",
        "I can hold and drag the slider to rate on a scale of 1 to 5"
        };

    private int[] answers = new int[2];

    // Start is called before the first frame update
    void Start()
    {
        // Get the root transform (example objects)
        Transform exampleObjects = gameObject.transform.root;

        // Get references to components belonging to this RatingTablet
        questionText = exampleObjects.Find("RatingTablet/RatingCanvas/QuestionText").GetComponent<TextMeshProUGUI>();
        //questionText = GameObject.Find("QuestionText").GetComponent<TextMeshProUGUI>();
        pageIndexText = exampleObjects.Find("RatingTablet/RatingCanvas/PageIndexText").GetComponent<TextMeshProUGUI>();
        //pageIndexText = GameObject.Find("PageIndexText").GetComponent<TextMeshProUGUI>();
        nextButtonText = exampleObjects.Find("RatingTablet/RatingCanvas/NextButton/NextButtonText").GetComponent<TextMeshProUGUI>();
        //nextButtonText = GameObject.Find("NextButtonText").GetComponent<TextMeshProUGUI>();
        backButton = exampleObjects.Find("RatingTablet/RatingCanvas/BackButtonRatingCanvas").gameObject;
        //backButton = GameObject.Find("BackButtonRatingCanvas");
        slider = exampleObjects.Find("RatingTablet/RatingCanvas/RatingSlider").GetComponent<Slider>();
        //slider = GameObject.Find("RatingSlider").GetComponent<Slider>();
        mainCanvas = exampleObjects.Find("RatingTablet/RatingCanvas").gameObject;
        //mainCanvas = GameObject.Find("RatingCanvas");
        completeCanvas = exampleObjects.Find("RatingTablet/CompleteCanvas").gameObject;
        //completeCanvas = GameObject.Find("CompleteCanvas");
        mainCanvas.SetActive(true);
        completeCanvas.SetActive(false);
        backButton.SetActive(false);
    }

    // Update is called once per frame
    public void Update()
    {
        if (pageIndex < questions.Length)
        {
            questionText.text = questions[pageIndex];
            pageIndexText.text = pageIndex + 1 + "/" + questions.Length;
        }
    }

    public void previousPage()
    {
        answers[pageIndex] = (int)slider.value;

        if (pageIndex > 0)
        {
            pageIndex--;
        }

        if (pageIndex == 0)
        {
            backButton.SetActive(false);
        }

        nextButtonText.text = "Next";
    }

    public void nextPage()
    {
        Debug.Log((int)slider.value);
        answers[pageIndex] = (int)slider.value;

        if (pageIndex < questions.Length - 1)
        {
            pageIndex++;
        }
        else
        {
            showCompletePage();
        }

        if (pageIndex == questions.Length - 1)
        {
            nextButtonText.text = "Submit";
        }

        if (pageIndex > 0)
        {
            backButton.SetActive(true);
        }
    }

    private void showCompletePage()
    {
        // writeToFile();
        Debug.Log("complete");
        mainCanvas.SetActive(false);
        completeCanvas.SetActive(true);
    }

    private void writeToFile()
    {
        using (StreamWriter writer = new StreamWriter("results.txt", true))
        {
            string lineToWrite = "";
            int lengthOfArray = answers.Length;
            for (int i = 0; i < answers.Length; i++)
            {
                if (i == answers.Length - 1)
                {
                    lineToWrite += answers[i];
                }
                else
                {
                    lineToWrite += answers[i] + ",";
                }

            }
            writer.WriteLine(lineToWrite);
            writer.Flush();
            writer.Close();
        }
    }
}
