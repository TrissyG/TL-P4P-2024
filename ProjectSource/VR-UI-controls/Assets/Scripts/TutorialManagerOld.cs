// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.XR;
// using UnityEngine.SceneManagement;

// // reference https://www.youtube.com/watch?v=a1RFxtuTVsk

// public class TutorialManager : MonoBehaviour
// {
//     public GameObject[] popups;
//     public GameObject exampleCube;
//     public GameObject exampleRatingTablet;
//     public GameObject exampleAudioObject;

//     private GameObject beginButton;

//     private int popupIndex;

//     private static int GRAB_OBJECT = 2;
//     private static int QUESTIONNAIRE_OBJECT = 7;
//     private static int SOUND_EXAMPLE = 9;
//     private static int TUTORIAL_COMPLETE = 12;
//     // Start is called before the first frame update
//     void Awake()
//     {
//         // ensure initialised to false
//         exampleRatingTablet.SetActive(false);
//         exampleCube.SetActive(false);
//         exampleAudioObject.SetActive(false);

//         // Get a reference to the 'Begin' button
//         if (gameObject.transform.root.Find("Canvas/Tutorial12 - Complete/BeginButton") != null) {
//             beginButton = gameObject.transform.root.Find("Canvas/Tutorial12 - Complete/BeginButton").gameObject;
//         }
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         for (int i = 0; i < popups.Length; i++)
//         {
//             if (i == popupIndex)
//             {
//                 popups[i].SetActive(true);
//             } else {
//                 popups[i].SetActive(false);
//             }
//         }

//         if (popupIndex >= GRAB_OBJECT && popupIndex < QUESTIONNAIRE_OBJECT)
//         {
//             exampleCube.SetActive(true);
//             exampleRatingTablet.SetActive(false);
//         } else if (popupIndex >= QUESTIONNAIRE_OBJECT && popupIndex < SOUND_EXAMPLE)
//         {
//             exampleRatingTablet.SetActive(true);
//             exampleCube.SetActive(false);
//         }

//         if (popupIndex == SOUND_EXAMPLE) {
//             exampleRatingTablet.SetActive(false);
//             exampleCube.SetActive(false);
//             exampleAudioObject.SetActive(true);
//         } else {
//             exampleAudioObject.SetActive(false);
//         }

//         if (popupIndex == TUTORIAL_COMPLETE) {
//             // Begin button deliberately disabled here for user study
//             // Only the spectator is able to change scenes.
//             if (beginButton != null) {
//                 beginButton.SetActive(false);
//             }
//         }

//     }

//     public void nextTutorialPage()
//     {
//         if (popupIndex < popups.Length - 1)
//         {
//             popupIndex++;
//         }
//     }

//     public void previousTutorialPage()
//     {
//         if (popupIndex > 0)
//         {
//             popupIndex--;
//         }
//     }

//     public void toFirstScene()
//     {
//         SceneManager.LoadScene("BlankWorld");
//     }
// }
