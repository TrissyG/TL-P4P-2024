using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System;

public class DataLoggingManagerTutorialScene : MonoBehaviour
{

    private GameObject cube;
    public GameObject ratingTablet;
    private GameObject audioObject;
    private Transform camera;
    private readonly string LOGGING_DIRECTORY = "LoggingData";
    private string path;
    private string ratingTabletPath;
    private string cameraPath;
    private string cubePath;
    private string audioObjectPath;
    private bool grabbedRatingTablet;
    private bool grabbedCube;
    private bool grabbedAudioObject;
    private bool headlocked;

    private enum ObjectName
    {
        Cube,
        RatingTablet,
        AudioObject,
    }
    // Start is called before the first frame update
    void Start()
    {
        cube = GameObject.Find("ExampleCube");
        audioObject = GameObject.Find("ExampleAudioObject");
        camera = Camera.main.transform;

        initiateCsvFile();

        // calls logPositions every half second
        InvokeRepeating("logPositions", 0.0f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void logPositions()
    {
        DateTime time = DateTime.Now;

        if (ratingTablet != null)
        {
            Vector3 position = ratingTablet.transform.position;
            Quaternion rotation = ratingTablet.transform.rotation;
            using (StreamWriter writer = new StreamWriter(ratingTabletPath, true))
            {
                string lineToWrite = "";

                //https://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager.GetActiveScene.html
                lineToWrite += SceneManager.GetActiveScene().name + ",";

                lineToWrite += $"{position.x},{position.y},{position.z},{rotation.x},{rotation.y},{rotation.z},{rotation.w},{grabbedRatingTablet.ToString()},{time}";

                writer.WriteLine(lineToWrite);
                writer.Flush();
                writer.Close();
            }
        }

        if (cube != null)
        {
            Vector3 cubePosition = cube.transform.position;
            Quaternion cubeRotation = cube.transform.rotation;
            using (StreamWriter cubeWriter = new StreamWriter(cubePath, true))
            {
                string lineToWrite = "";

                lineToWrite += SceneManager.GetActiveScene().name + ",";

                lineToWrite += $"{cubePosition.x},{cubePosition.y},{cubePosition.z},{cubeRotation.x},{cubeRotation.y},{cubeRotation.z},{cubeRotation.w},{grabbedCube.ToString()},{headlocked.ToString()},{time}";

                cubeWriter.WriteLine(lineToWrite);
                cubeWriter.Flush();
                cubeWriter.Close();
            }
        }

        if (audioObject != null)
        {
            Vector3 audioObjectPosition = audioObject.transform.position;
            Quaternion audioObjectRotation = audioObject.transform.rotation;
            using (StreamWriter audioObjectWriter = new StreamWriter(audioObjectPath, true))
            {
                string lineToWrite = "";

                lineToWrite += SceneManager.GetActiveScene().name + ",";

                lineToWrite += $"{audioObjectPosition.x},{audioObjectPosition.y},{audioObjectPosition.z},{audioObjectRotation.x},{audioObjectRotation.y},{audioObjectRotation.z},{audioObjectRotation.w},{grabbedAudioObject.ToString()},{headlocked.ToString()},{time}";

                audioObjectWriter.WriteLine(lineToWrite);
                audioObjectWriter.Flush();
                audioObjectWriter.Close();
            }
        }

        Vector3 cameraPosition = camera.position;
        Quaternion cameraRotation = camera.rotation;
        using (StreamWriter cameraWriter = new StreamWriter(cameraPath, true))
        {
            string lineToWrite = "";

            lineToWrite += SceneManager.GetActiveScene().name + ",";

            lineToWrite += $"{cameraPosition.x},{cameraPosition.y},{cameraPosition.z},{cameraRotation.x},{cameraRotation.y},{cameraRotation.z},{cameraRotation.w},{time}";

            cameraWriter.WriteLine(lineToWrite);
            cameraWriter.Flush();
            cameraWriter.Close();
        }
    }

    private void writeEventToFile(string eventName, ObjectName objectName)
    {
        DateTime time = DateTime.Now;

        // https://docs.unity3d.com/ScriptReference/Transform.InverseTransformPoint.html
        Vector3 position = new Vector3(0, 0, 0);
        if (objectName == ObjectName.Cube)
        {
            position = camera.InverseTransformPoint(cube.transform.position);
        }
        else if (objectName == ObjectName.AudioObject)
        {
            position = camera.InverseTransformPoint(audioObject.transform.position);
        }
        else if (objectName == ObjectName.RatingTablet)
        {
            position = camera.InverseTransformPoint(ratingTablet.transform.position);
        }

        using (StreamWriter writer = new StreamWriter(path, true))
        {
            string lineToWrite = "";

            //https://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager.GetActiveScene.html
            lineToWrite += SceneManager.GetActiveScene().name + ",";

            lineToWrite += $"{eventName},{objectName},{position.x},{position.y},{position.z},{time}";

            writer.WriteLine(lineToWrite);
            writer.Flush();
            writer.Close();
        }
    }

    private void initiateCsvFile()
    {
        if (!Directory.Exists(LOGGING_DIRECTORY))
        {
            Directory.CreateDirectory(LOGGING_DIRECTORY);
        }

        string time = DateTime.Now.ToString("yyyy-MM-dd--HH-mm-ss"); ;

        path = Path.Combine(LOGGING_DIRECTORY, "tutorial-events-" + time + ".csv");
        ratingTabletPath = Path.Combine(LOGGING_DIRECTORY, "tutorial-rating-tablet-" + time + ".csv");
        cubePath = Path.Combine(LOGGING_DIRECTORY, "tutorial-cube-" + time + ".csv");
        audioObjectPath = Path.Combine(LOGGING_DIRECTORY, "tutorial-audio-object-" + time + ".csv");
        cameraPath = Path.Combine(LOGGING_DIRECTORY, "tutorial-camera-" + time + ".csv");

        if (!ExperimentData.loggingFileHeader)
        {
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                string lineToWrite = "Scenario,Event,Object,x,y,z,Time";

                writer.WriteLine(lineToWrite);
                writer.Flush();
                writer.Close();
            }

            using (StreamWriter ratingTabletWriter = new StreamWriter(ratingTabletPath, true))
            {
                string lineToWrite = "Scenario,positionX,positionY,positionZ,rotationX,rotationY,rotationZ,rotationW,Grabbed,Time";

                ratingTabletWriter.WriteLine(lineToWrite);
                ratingTabletWriter.Flush();
                ratingTabletWriter.Close();
            }

            using (StreamWriter cubeWriter = new StreamWriter(cubePath, true))
            {
                string lineToWrite = "Scenario,positionX,positionY,positionZ,rotationX,rotationY,rotationZ,rotationW,Grabbed,Headlocked,Time";

                cubeWriter.WriteLine(lineToWrite);
                cubeWriter.Flush();
                cubeWriter.Close();
            }

            using (StreamWriter audioObjectWriter = new StreamWriter(audioObjectPath, true))
            {
                string lineToWrite = "Scenario,positionX,positionY,positionZ,rotationX,rotationY,rotationZ,rotationW,Grabbed,Headlocked,Time";

                audioObjectWriter.WriteLine(lineToWrite);
                audioObjectWriter.Flush();
                audioObjectWriter.Close();
            }

            using (StreamWriter cameraWriter = new StreamWriter(cameraPath, true))
            {
                string lineToWrite = "Scenario,positionX,positionY,positionZ,rotationX,rotationY,rotationZ,rotationW,Time";

                cameraWriter.WriteLine(lineToWrite);
                cameraWriter.Flush();
                cameraWriter.Close();
            }
            ExperimentData.loggingFileHeader = true;
        }

    }

    public void grabEnteredCube()
    {
        grabbedCube = true;
        writeEventToFile("grabEntered", ObjectName.Cube);
    }

    public void grabExitedCube()
    {
        grabbedCube = false;
        writeEventToFile("grabExited", ObjectName.Cube);
    }

    public void grabEnteredAudioObject()
    {
        grabbedAudioObject = true;
        writeEventToFile("grabEntered", ObjectName.AudioObject);
    }

    public void grabExitedAudioObject()
    {
        grabbedAudioObject = false;
        writeEventToFile("grabExited", ObjectName.AudioObject);
    }

    public void headlockedCube()
    {
        headlocked = !headlocked;
        writeEventToFile("headlockedObject", ObjectName.Cube);
    }

    public void headlockedAudioObject()
    {
        headlocked = !headlocked;
        writeEventToFile("headlockedObject", ObjectName.AudioObject);
    }

    public void grabEnteredRatingTablet()
    {
        grabbedRatingTablet = true;
        writeEventToFile("grabEntered", ObjectName.RatingTablet);
    }

    public void grabExitedRatingTablet()
    {
        grabbedRatingTablet = false;
        writeEventToFile("grabExited", ObjectName.RatingTablet);
    }
}
