using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System;



public class RatingTabletManager : MonoBehaviour
{
    private TextMeshProUGUI questionText;
    private TextMeshProUGUI pageIndexText;
    private TextMeshProUGUI nextButtonText;
    private GameObject backButton;
    private Slider slider;
    private GameObject mainCanvas;
    private GameObject completeCanvas;
    private GameObject nextScenarioButton;

    private int pageIndex = 0;
    private string[] questions = new string[]{
        /* OLD QUESTIONS
        "I enjoyed this VR environment",
        "This VR environment felt very realistic",
        "The sound seemed to be emitted by the radio",
        "When moving the radio, I noticed the sound would move with it",
        "I felt very relaxed during this VR experience",
        "Being able to interact with the objects increased the realism of the VR environment",
        "Changing the position of the radio changed the effectiveness of the masking sound"
        */
        "I enjoyed this environment",
        "This environment felt very realistic",
        "I felt very relaxed",
        "I could easily identify the location of sounds",
        "I could easily focus on the masking sounds"
        };

    private int[] answers = new int[5];

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
        if (ratingTablet.Find("CompleteCanvas/NextButton") != null) {
            nextScenarioButton = ratingTablet.Find("CompleteCanvas/NextButton").gameObject;
        }
        mainCanvas.SetActive(true);
        completeCanvas.SetActive(false);
        backButton.SetActive(false);
        initiateCsvFile();

        startTime = DateTime.Now;

        // init all answers to -1
        for (int i = 0; i < answers.Length; i++) {
            answers[i] = -1;
        }

        // init slider to middle (3)
        slider.value = 3;
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
            // restore previous answer
            slider.value = answers[pageIndex];
        }

        if (pageIndex == 0)
        {
            backButton.SetActive(false);
        }

        nextButtonText.text = "Next";
    }

    public void nextPage()
    {
        //Debug.Log((int)slider.value);
        answers[pageIndex] = (int)slider.value;

        if (pageIndex < questions.Length - 1)
        {
            pageIndex++;
            if (answers[pageIndex] == -1) {
                // set slider to middle (3)
                slider.value = 3;
            } else {
                // restore previous answer
                slider.value = answers[pageIndex];
            }
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
        Debug.Log("Rating tablet completed");
        mainCanvas.SetActive(false);
        completeCanvas.SetActive(true);
        // Next scenario button is deliberately disabled here (for user testing purposes)
        // Only spectator is able to change scenes
        if (nextScenarioButton != null) {
            nextScenarioButton.SetActive(false);
        }
    }

    private void writeToFile()
    {
        endTime = DateTime.Now;

        using (StreamWriter writer = new StreamWriter(path, true))
        {
            string lineToWrite = "";

            //https://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager.GetActiveScene.html
            lineToWrite += SceneManager.GetActiveScene().name;

            // include whether environment sounds are playing along with the scene name
            bool environmentIsPlaying;
            Settings.Instance.GetValue("environmentIsPlaying", out environmentIsPlaying);
            if (environmentIsPlaying) {
                lineToWrite += "(envSound=true)";
            } else {
                lineToWrite += "(envSound=false)";
            }
            lineToWrite += ",";

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

        if (ExperimentData.Filename == null)
        {
            ExperimentData.ExperimentStartTime = DateTime.Now.ToString("yyyy-MM-dd--HH-mm-ss");
            ExperimentData.Filename = "results-" + ExperimentData.ExperimentStartTime + ".csv";
            firstScene = true;
        }

        path = Path.Combine(LOGGING_DIRECTORY, ExperimentData.Filename);

        if (firstScene)
        {
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                string lineToWrite = "Scenario,";
                for (int i = 0; i < questions.Length; i++) {
                    lineToWrite += questions[i] + ",";
                }
                lineToWrite += "startTime,endTime";

                writer.WriteLine(lineToWrite);
                writer.Flush();
                writer.Close();
            }
        }

    }
}
