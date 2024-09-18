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
    public InputDevice _rightController;
    public InputDevice _leftController;
    public InputDevice _HMD;

    public XRInteractorLineVisual rightLineVisual;
    public XRInteractorLineVisual leftLineVisual;
    public Gradient invalidColorGradient;
    public Gradient validColorGradient;

    void Start()
    {
        // Start the coroutine to wait for 5 seconds
        StartCoroutine(WaitForBoot());

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

        if (!_rightController.isValid)
            InitializeInputDevice(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right, ref _rightController);
            rightLineVisual = _rightController.GetComponent<XRInteractorLineVisual>();
            invalidColorGradient = rightLineVisual.invalidColorGradient;
            validColorGradient = rightLineVisual.validColorGradient;
        if (!_leftController.isValid)
            InitializeInputDevice(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left, ref _leftController);
            leftLineVisual = _leftController.GetComponent<XRInteractorLineVisual>();
        if (!_HMD.isValid)
            InitializeInputDevice(InputDeviceCharacteristics.HeadMounted, ref _HMD);

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

public void activateControllerObjectDetection()
    {
        foreach (GameObject controller in controllers)
        {
            XRInteractorLineVisual lineVisual = controller.GetComponent<XRInteractorLineVisual>();
            if (lineVisual != null)
            {
                // Restore the original validColorGradient if needed
                lineVisual.validColorGradient.set = validColorGradient;
            }
        }
    }

    public void deactivateControllerObjectDetection()
    {
        foreach (GameObject controller in controllers)
        {
            XRInteractorLineVisual lineVisual = controller.GetComponent<XRInteractorLineVisual>();
            if (lineVisual != null)
            {
                // Set the validColorGradient to the same as the invalidColorGradient
                lineVisual.validColorGradient.set = invalidColorGradient;
            }
        }
    }


}
