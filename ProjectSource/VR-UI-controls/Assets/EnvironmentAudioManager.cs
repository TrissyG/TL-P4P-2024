using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class EnvironmentAudioManager : MonoBehaviour
{
    private IEnvironmentSound[] sounds;
    public bool playSound;
    public AudioMixerGroup environmentMixerGroup;


    private void Start()
    {
        // get controllable components in all environment sounds (children of this GameObject)
        sounds = transform.GetComponentsInChildren<IEnvironmentSound>();
        // TODO: group sounds into categories? (birds/ambient)

        // ensure all environment sounds are going through the correct audio mixer group
        foreach (AudioSource audioSource in transform.GetComponentsInChildren<AudioSource>()) {
            audioSource.outputAudioMixerGroup = environmentMixerGroup;
        }
    }

    private void Update()
    {
        foreach (IEnvironmentSound sound in sounds) {
            if (playSound) {
                sound.SetEnabled(true);
            } else {
                sound.SetEnabled(false);
            }
        }
    }

    // TODO: create methods allowing the UI to control audio
}
