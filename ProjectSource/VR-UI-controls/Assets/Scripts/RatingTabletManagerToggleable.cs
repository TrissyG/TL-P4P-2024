using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System;

public class RatingTabletManagerToggleable : MonoBehaviour
{
    private TextMeshProUGUI questionText;
    private TextMeshProUGUI pageIndexText;
    private TextMeshProUGUI nextButtonText;
    private GameObject backButton;
    private Slider slider;
    private GameObject mainCanvas;
    private GameObject completeCanvas;

    private TextMeshProUGUI scaleText1;
    private TextMeshProUGUI scaleText2;
    private TextMeshProUGUI scaleText3;
    private TextMeshProUGUI scaleText4;
    private TextMeshProUGUI scaleText5;

    private int pageIndex = 0;
    // private string[] questions = new string[]{
    //     "How much of a problem is your tinnitus at present?",
    //     "How STRONG or LOUD is tinnitus at present?",
    //     "How UNCOMFORTABLE is your tinnitus at present, if everything around you is quiet?",
    //     "How ANNOYING is your tinnitus at present?",
    //     "How easy is it for you to IGNORE your tinnitus at present?",
    //     "How UNPLEASANT is your tinnitus at present?"
    //     };

    private string[] questions = new string[]{
        // RSQ questions - https://www.nature.com/articles/s41598-022-20524-w/tables/1
        "My breathing is faster than usual.", // 1
        "My heart is beating faster than usual", // 2
        "My muscles feel tense and cramped.", // 3
        "My muscles feel relaxed.", // 4
        "My muscles feel loose.", // 5
        "I'm feeling very relaxed.", // 6
        "Right now, I am completely calm.", // 7
        "I'm feeling sleepy and tired.", // 8
        "I'm about to doze off.", // 9
        "I'm feeling refreshed and awake.", // 10
        // IMI - Interest/Enjoyment
        "I enjoyed doing this activity very much.", // 11
        "This activity was fun to do.", // 12
        "I thought this was a boring activity.", // 13
        "This activity did not hold my attention at all.", // 14
        "I would describe this activity as very interesting.", // 15
        "I thought this activity was quite enjoyable.", // 16
        "While I was doing this activity, I was thinking about how much I enjoyed it.", // 17
        // IMI - Pressure/Tension
        "I did not feel nervous at all while doing this.", // 18
        "I felt very tense while doing this activity.", // 19
        "I was very relaxed in doing these.", // 20
        "I was anxious while working on this task.", // 21
        "I felt pressured while doing these." // 22
        };

    private int[] answers = new int[10];

    private string filename;
    private string path;
    private readonly string QUESTIONNAIRE_RESULTS_DIRECTORY = "QuestionnaireResults";
    private readonly string LOGGING_DIRECTORY = "LoggingData";

    private DateTime startTime;
    private DateTime endTime;

    // Start is called before the first frame update
    void Start()
    {
        // Get the root transform (rating tablet)
        Transform ratingTablet = gameObject.transform.root;

        // Get references to components belonging to this RatingTablet
        questionText = ratingTablet.Find("RatingCanvas/QuestionText").GetComponent<TextMeshProUGUI>();
        //questionText = GameObject.Find("QuestionText").GetComponent<TextMeshProUGUI>();
        pageIndexText = ratingTablet.Find("RatingCanvas/PageIndexText").GetComponent<TextMeshProUGUI>();
        //pageIndexText = GameObject.Find("PageIndexText").GetComponent<TextMeshProUGUI>();
        nextButtonText = ratingTablet.Find("RatingCanvas/NextButton/NextButtonText").GetComponent<TextMeshProUGUI>();
        //nextButtonText = GameObject.Find("NextButtonText").GetComponent<TextMeshProUGUI>();
        backButton = ratingTablet.Find("RatingCanvas/BackButtonRatingCanvas").gameObject;
        //backButton = GameObject.Find("BackButtonRatingCanvas");
        slider = ratingTablet.Find("RatingCanvas/RatingSlider").GetComponent<Slider>();
        //slider = GameObject.Find("RatingSlider").GetComponent<Slider>();
        mainCanvas = ratingTablet.Find("RatingCanvas").gameObject;
        //mainCanvas = GameObject.Find("RatingCanvas");
        completeCanvas = ratingTablet.Find("CompleteCanvas").gameObject;
        //completeCanvas = GameObject.Find("CompleteCanvas");
        scaleText1 = ratingTablet.Find("RatingCanvas/ScaleText1").GetComponent<TextMeshProUGUI>();
        scaleText2 = ratingTablet.Find("RatingCanvas/ScaleText2").GetComponent<TextMeshProUGUI>();
        scaleText3 = ratingTablet.Find("RatingCanvas/ScaleText3").GetComponent<TextMeshProUGUI>();
        scaleText4 = ratingTablet.Find("RatingCanvas/ScaleText4").GetComponent<TextMeshProUGUI>();
        scaleText5 = ratingTablet.Find("RatingCanvas/ScaleText5").GetComponent<TextMeshProUGUI>();
        mainCanvas.SetActive(true);
        completeCanvas.SetActive(false);
        backButton.SetActive(false);
        initiateCsvFile();

        startTime = DateTime.Now;
    }

    // Update is called once per frame
    public void Update()
    {
        if (pageIndex < questions.Length)
        {
            questionText.text = questions[pageIndex];
            pageIndexText.text = pageIndex + 1 + "/" + questions.Length;
        }

        // Certain pages have different rating scales
        // if (pageIndex == 0) {
        //     slider.maxValue = 5;
        //     scaleText1.text = "Not\na\nproblem";
        //     scaleText2.text = "A\nsmall\nproblem";
        //     scaleText3.text = "A\nmoderate\nproblem";
        //     scaleText4.text = "A\nbig\nproblem";
        //     scaleText5.text = "A\nvery big\nproblem";
        // } else if (pageIndex == 4){
        //     slider.maxValue = 10;
        //     scaleText1.text = "\nVery easy\n1";
        //     scaleText2.text = " ";
        //     scaleText3.text = " ";
        //     scaleText4.text = " ";
        //     scaleText5.text = "\nImpossible\n10";
        // } else {
        //     slider.maxValue = 10;
        //     scaleText1.text = "\nNot at all\n1";
        //     scaleText2.text = " ";
        //     scaleText3.text = " ";
        //     scaleText4.text = " ";
        //     scaleText5.text = "\nExtremely\n10";
        // }

        if (pageIndex == 0) {
            // First 10 questions are the RSQ
                    slider.maxValue = 5;
        scaleText1.text = "Not\ncorrect\nat all";
        scaleText2.text = "Rather\nnot\ncorrect";
        scaleText3.text = "Neither\nnor";
        scaleText4.text = "Rather\ncorrect";
        scaleText5.text = "Entirely\ncorrect";

        } else if (pageIndex == 10) {
            // 11th question, start of IMI
            slider.maxValue = 7; // IMI rating goes from 1 to 7
            scaleText1.text = "Not at\nall true\n1";
            scaleText2.text = "";
            scaleText3.text = "Somewhat\ntrue\n4";
            scaleText4.text = "";
            scaleText5.text = "Very\ntrue\n7";
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
        writeToFile();
        Debug.Log("complete");
        mainCanvas.SetActive(false);
        completeCanvas.SetActive(true);
    }

    private void writeToFile()
    {
        endTime = DateTime.Now;

        using (StreamWriter writer = new StreamWriter(path, true))
        {
            string lineToWrite = "";

            //https://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager.GetActiveScene.html
            lineToWrite += SceneManager.GetActiveScene().name + ",";

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

            lineToWrite += "," + startTime + "," + endTime;

            writer.WriteLine(lineToWrite);
            writer.Flush();
            writer.Close();
        }
    }

    // reference: https://stackoverflow.com/questions/12500091/datetime-tostring-format-that-can-be-used-in-a-filename-or-extension
    private void initiateCsvFile()
    {
        if (!Directory.Exists(LOGGING_DIRECTORY))
        {
            Directory.CreateDirectory(LOGGING_DIRECTORY);
        }

        bool firstScene = false;

        if (TSNSExperimentData.Filename == null)
        {
            TSNSExperimentData.ExperimentStartTime = DateTime.Now.ToString("yyyy-MM-dd--HH-mm-ss");
            TSNSExperimentData.Filename = "TSNS_results-" + TSNSExperimentData.ExperimentStartTime + ".csv";
            firstScene = true;
        }

        path = Path.Combine(LOGGING_DIRECTORY, TSNSExperimentData.Filename);

        if (firstScene)
        {
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                string lineToWrite = "Scenario,1,2,3,4,5,6,startTime,endTime";

                writer.WriteLine(lineToWrite);
                writer.Flush();
                writer.Close();
            }
        }

    }
}
