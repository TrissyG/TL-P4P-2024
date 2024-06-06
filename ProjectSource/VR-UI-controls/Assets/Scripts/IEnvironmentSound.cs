using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnvironmentSound
{
    // Returns true if the sound object is enabled
    bool isEnabled();

    // Allow the sound object to play sounds
    void SetEnabled(bool value);

    // Returns true if sound is currently playing
    bool isPlaying();

    // Get the current volume
    float GetVolume();

    // Set the current volume
    void SetVolume(float value);
}
