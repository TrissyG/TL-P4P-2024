/** 
This script is sourced from YouTube channel FistFullOfShrimp's template, which can be found in the link below:
https://github.com/Fist-Full-of-Shrimp/FFOS-Unity-VR-Template
It is usable under the MIT License.

MIT License

Copyright (c) 2022 Shelby Drabant

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class InputData : MonoBehaviour
{
    private bool _isInitialized = false;
    private List<GameObject> controllers = new List<GameObject>();
    public InputDevice _rightController;
    public GameObject _rightControllerObject;
    public InputDevice _leftController;
    public GameObject _leftControllerObject;
    public InputDevice _HMD;

    public UnityEngine.XR.Interaction.Toolkit.Interactors.Visuals.XRInteractorLineVisual rightLineVisual;
    public UnityEngine.XR.Interaction.Toolkit.Interactors.Visuals.XRInteractorLineVisual leftLineVisual;
    public Gradient invalidColorGradient;
    public Gradient validColorGradient;

    void Start()
    {
        // Start the coroutine to wait for 5 seconds
        StartCoroutine(WaitForBoot());
        _rightControllerObject = GameObject.Find("RightHand Controller");
        controllers.Add(_rightControllerObject);
        _leftControllerObject = GameObject.Find("LeftHand Controller");
        controllers.Add(_leftControllerObject);
    }

    IEnumerator WaitForBoot()
    {
        // Wait for 5 seconds
        yield return new WaitForSeconds(5f);
        _isInitialized = true;
    }


    void Update()
    {
        if (!_isInitialized)
            return;
        if (!_rightController.isValid || !_leftController.isValid || !_HMD.isValid)
            InitializeInputDevices();


    }
    private void InitializeInputDevices()
    {
        Debug.Log("Initializing input devices");
        if (!_rightController.isValid)
        {
            Debug.Log("Initializing right controller");
            InitializeInputDevice(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right, ref _rightController);
            // rightLineVisual = _rightControllerObject.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.Visuals.XRInteractorLineVisual>();
            // validColorGradient = rightLineVisual.validColorGradient;
        }

        if (!_leftController.isValid)
        {
            Debug.Log("Initializing left controller");
            InitializeInputDevice(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left, ref _leftController);
            //leftLineVisual = _leftControllerObject.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.Visuals.XRInteractorLineVisual>();
        }

        if (!_HMD.isValid)
        {
            Debug.Log("Initializing HMD");
            InitializeInputDevice(InputDeviceCharacteristics.HeadMounted, ref _HMD);
        }

    }

    private void InitializeInputDevice(InputDeviceCharacteristics inputCharacteristics, ref InputDevice inputDevice)
    {
        List<InputDevice> devices = new List<InputDevice>();
        //Call InputDevices to see if it can find any devices with the characteristics we're looking for
        InputDevices.GetDevicesWithCharacteristics(inputCharacteristics, devices);

        //Our hands might not be active and so they will not be generated from the search.
        //We check if any devices are found here to avoid errors.
        if (devices.Count > 0)
        {
            inputDevice = devices[0];
        }
        //Debug.Log("Input device initialized");
        // List all the input devices
        foreach (var device in devices)
        {
            Debug.Log("Device found with name: " + device.name + " and role: " + device.role);
        }

    }

    // private void ChangeRayColor(Gradient gradient)
    // {
    //     if (lineVisual != null)
    //     {
    //         lineVisual.validColorGradient = gradient;
    //     }
    // }

    // public void activateControllerObjectDetection()
    // {

    //     foreach (GameObject controller in controllers)
    //     {
    //         try
    //         {
    //             UnityEngine.XR.Interaction.Toolkit.Interactors.Visuals.XRInteractorLineVisual lineVisual = controller.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.Visuals.XRInteractorLineVisual>();
    //             if (lineVisual != null)
    //             {
    //                 //lineVisual.validColorGradient
    //             }
    //         }
    //         catch (System.Exception e)
    //         {
    //             Debug.Log("Error: " + e);
    //             continue;
    //         }
    //     }
    // }

    // public void deactivateControllerObjectDetection()
    // {
    //     foreach (GameObject controller in controllers)
    //     {
    //         UnityEngine.XR.Interaction.Toolkit.Interactors.Visuals.XRInteractorLineVisual lineVisual = controller.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.Visuals.XRInteractorLineVisual>();
    //         if (lineVisual != null)
    //         {
    //             // Set the validColorGradient to the same as the invalidColorGradient
    //             lineVisual.validColorGradient = invalidColorGradient;
    //         }
    //     }
    // }

}
