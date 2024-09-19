using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System;

public class DataLoggingManager : MonoBehaviour
{
    private GameObject radio;
    public GameObject ratingTablet;
    private Transform camera;
    private readonly string LOGGING_DIRECTORY = "LoggingData";
    private string path;
    private string ratingTabletPath;
    private string cameraPath;
    private string radioPath;

    private string locationingPath;

    private bool grabbedRatingTablet;
    private bool grabbedRadio;
    private bool headlocked;
    // LocationingMode data captured
    private bool activatedLocationingMode; // from SpectatorUI

    private int RadioPositionIndex; // from SpectatorUI
    private bool RadioVisible = true; // from SpectatorUI
    private bool AudioSourceVisible;// from SpectatorUI
    private float offsetRadius; // from SpectatorUI
    private float offsetAzimuth; // from SpectatorUI
    private float offsetInclination; // from SpectatorUI
    private bool pressedLocationingButton; // from AimingDistanceManager
    private Vector3 audioSourceLocation; // from AimingDistanceManager
    private Vector3 rayOriginPoint; // from AimingDistanceManager
    private Vector3 rayIntersectPoint; // from AimingDistanceManager
    private float displacementAngleFromAudioSource; // from AimingDistanceManager
    private float displacementAngleFromRadio; // from AimingDistanceManager
    private int OffsetPresetIndex; // from AimingDistanceManager


    private enum ObjectName
    {
        Radio,
        RatingTablet,
        RightHandController,
        SpectatorUI

    }

    // Start is called before the first frame update
    void Start()
    {
        radio = GameObject.Find("Radio");
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

        Vector3 radioPosition = radio.transform.position;
        Quaternion radioRotation = radio.transform.rotation;
        using (StreamWriter radioWriter = new StreamWriter(radioPath, true))
        {
            string lineToWrite = "";

            lineToWrite += SceneManager.GetActiveScene().name + ",";

            lineToWrite += $"{radioPosition.x},{radioPosition.y},{radioPosition.z},{radioRotation.x},{radioRotation.y},{radioRotation.z},{radioRotation.w},{grabbedRadio.ToString()},{headlocked.ToString()},{time}";

            radioWriter.WriteLine(lineToWrite);
            radioWriter.Flush();
            radioWriter.Close();
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

        if (activatedLocationingMode && pressedLocationingButton)
        {
            LogLocationingData();
            pressedLocationingButton = false;
        }
    }

    public void LogLocationingData()
    {
        DateTime time = DateTime.Now;
        Vector3 radioPosition = radio.transform.position;
        Quaternion radioRotation = radio.transform.rotation;
        using (StreamWriter writer = new StreamWriter(locationingPath, true))
        {
            string lineToWrite = $"{SceneManager.GetActiveScene().name},{activatedLocationingMode},{pressedLocationingButton},{OffsetPresetIndex}{RadioPositionIndex},{RadioVisible},{AudioSourceVisible},{offsetRadius},{offsetAzimuth},{offsetInclination},{displacementAngleFromAudioSource},{displacementAngleFromRadio},{audioSourceLocation.x},{audioSourceLocation.y},{audioSourceLocation.z},{rayOriginPoint.x},{rayOriginPoint.y},{rayOriginPoint.z},{rayIntersectPoint.x},{rayIntersectPoint.y},{radioPosition.x},{radioPosition.y},{radioPosition.z},{time}";
            writer.WriteLine(lineToWrite);
            writer.Flush();
            writer.Close();
        }
    }

    private void writeEventToFile(string eventName, ObjectName objectName)
    {
        DateTime time = DateTime.Now;

        // https://docs.unity3d.com/ScriptReference/Transform.InverseTransformPoint.html
        Vector3 position = new Vector3(0, 0, 0);
        if (objectName == ObjectName.Radio)
        {
            position = camera.InverseTransformPoint(radio.transform.position);
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

        path = Path.Combine(LOGGING_DIRECTORY, "events-" + ExperimentData.ExperimentStartTime + ".csv");
        ratingTabletPath = Path.Combine(LOGGING_DIRECTORY, "rating-tablet-" + ExperimentData.ExperimentStartTime + ".csv");
        radioPath = Path.Combine(LOGGING_DIRECTORY, "radio-" + ExperimentData.ExperimentStartTime + ".csv");
        cameraPath = Path.Combine(LOGGING_DIRECTORY, "camera-" + ExperimentData.ExperimentStartTime + ".csv");
        locationingPath = Path.Combine(LOGGING_DIRECTORY, "locationing-" + ExperimentData.ExperimentStartTime + ".csv");

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

            using (StreamWriter radioWriter = new StreamWriter(radioPath, true))
            {
                string lineToWrite = "Scenario,positionX,positionY,positionZ,rotationX,rotationY,rotationZ,rotationW,Grabbed,Headlocked,Time";

                radioWriter.WriteLine(lineToWrite);
                radioWriter.Flush();
                radioWriter.Close();
            }

            using (StreamWriter cameraWriter = new StreamWriter(cameraPath, true))
            {
                string lineToWrite = "Scenario,positionX,positionY,positionZ,rotationX,rotationY,rotationZ,rotationW,Time";

                cameraWriter.WriteLine(lineToWrite);
                cameraWriter.Flush();
                cameraWriter.Close();
            }

            using (StreamWriter locationingWriter = new StreamWriter(locationingPath, true))
            {
                string lineToWrite = "Scenario,LocationingMode,PressedLocationingButton,OffsetPresetIndex,RadioPositionIndex,RadioVisible,AudioSourceVisible,offsetRadius,offsetAzimuth,offsetInclination,displacementAngleAS,displacementAngleR,AudioSourcePositionX,AudioSourcePositionY,AudioSourcePositionZ,rayOriginX,rayOriginY,rayOriginZ,rayDirectionX,rayDirectionY,rayDirectionZ,radioPositionX,radioPositionY,radioPositionZ,Time";

                locationingWriter.WriteLine(lineToWrite);
                locationingWriter.Flush();
                locationingWriter.Close();
            }
            ExperimentData.loggingFileHeader = true;
        }

    }

    public void grabEntered()
    {
        grabbedRadio = true;
        writeEventToFile("grabEntered", ObjectName.Radio);
    }

    public void grabExited()
    {
        grabbedRadio = false;
        writeEventToFile("grabExited", ObjectName.Radio);
    }

    public void headlockedObject()
    {
        headlocked = !headlocked;
        writeEventToFile("headlockedObject", ObjectName.Radio);
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
    // // locationing mode - when the user aims and clicks the x button, record the distance between the radio and 
    public void pressLocationingButton()
    {
        pressedLocationingButton = true;
        writeEventToFile("pressLocationingButton", ObjectName.RightHandController);
    }

    public void releaseLocationingButton()
    {
        pressedLocationingButton = false;
        writeEventToFile("releaseLocationingButton", ObjectName.RightHandController);
    }

    public void activateLocationingMode()
    {
        activatedLocationingMode = true;
        writeEventToFile("activateLocationingMode", ObjectName.SpectatorUI);
    }

    public void deactivateLocationingMode()
    {
        activatedLocationingMode = false;
        writeEventToFile("deactivateLocationingMode", ObjectName.SpectatorUI);
    }

    public void setRadioPositionIndex(int index)
    {
        RadioPositionIndex = index;
    }

    public void setRadioVisible(bool visible)
    {
        RadioVisible = visible;
    }

    public void setAudioSourceVisible(bool visible)
    {
        AudioSourceVisible = visible;
    }

    public void setAudioSourceOffset(float radius, float azimuth, float inclination)
    {
        offsetRadius = radius;
        offsetAzimuth = azimuth;
        offsetInclination = inclination;
    }

    public void setAudioSourceLocation(Vector3 location)
    {
        audioSourceLocation = location;
    }

    public void setRayOriginPoint(Vector3 point)
    {
        rayOriginPoint = point;
    }

    public void setRayIntersectPoint(Vector3 point)
    {
        rayIntersectPoint = point;
    }


    public void setDisplacementAngleFromAudioSource(float angle)
    {
        displacementAngleFromAudioSource = angle;
    }

    public void setDisplacementAngleFromRadio(float angle)
    {
        displacementAngleFromRadio = angle;
    }

    public void setOffsetPreset(int index)
    {
        OffsetPresetIndex = index;
    }




}
