using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundRandomiser : MonoBehaviour, IEnvironmentSound
{
    public AudioClip[] sounds;
    private AudioSource source;
    public float baseVolume;
    private float volumeChange = 0.15f;
    private float pitchChange = 0.15f;
    public float minInterval;
    public float maxInterval;
    private float soundRate;
    private Coroutine randomSoundGen;


    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        if (randomSoundGen == null) {
            randomSoundGen = StartCoroutine(playRandomSounds());
        }
    }

    private void OnEnable()
    {
        // Start playing sounds.
        if (randomSoundGen == null) {
            randomSoundGen = StartCoroutine(playRandomSounds());
        }
    }

    private void OnDisable()
    {
        // Stop playing sounds.
        StopCoroutine(randomSoundGen);
        randomSoundGen = null;
        source.Stop();
    }

    private IEnumerator playRandomSounds()
    {
        yield return null; // wait for Start()

        // don't always play the first sound immediately.
        float waitTime = Random.Range(-sounds[Random.Range(0, sounds.Length)].length, maxInterval - minInterval);
        if (waitTime > 0) {
            yield return new WaitForSeconds(waitTime);
        }

        while (true)
        {
            soundRate = Random.Range(minInterval, maxInterval);
            source.clip = sounds[Random.Range(0, sounds.Length)];
            source.volume = Random.Range(baseVolume - volumeChange, baseVolume);
            source.pitch = Random.Range(1 - pitchChange, 1 + pitchChange);
            source.Play();
            yield return new WaitForSeconds(source.clip.length + soundRate);
        }
        
    }

    // Interface methods (for AudioManager to control)
    public bool isEnabled()
    {
        return this.enabled;
    }

    public void SetEnabled(bool value)
    {
        this.enabled = value;
    }

    public bool isPlaying()
    {
        return source.isPlaying;
    }

    public float GetVolume()
    {
        return baseVolume;
    }

    public void SetVolume(float value)
    {
        baseVolume = value;
    }
}
