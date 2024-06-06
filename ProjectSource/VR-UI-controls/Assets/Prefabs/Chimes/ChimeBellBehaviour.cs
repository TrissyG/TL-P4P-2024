using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChimeBellBehaviour : MonoBehaviour
{
    private AudioSource audioSource;
    private Rigidbody rb;

    [Tooltip("Start the clip partway into its playback.")]
    public float audioClipStartTime = 0;

    // Defines the effect of collision force on audio playback
    public float strongImpactThreshold = 1;
    public float minimumImpactEffect = 0.5f;
    public float impactEffectFactor = 1f;
    public float minimumImpactClipStartTime = 3f;


    // Start is called before the first frame update
    void Start()
    {
        // Get a reference to this object's AudioSource, in order to play sounds
        audioSource = gameObject.GetComponent<AudioSource>();

        // Get a reference to this object's rigidbody, to control forces.
        rb = gameObject.GetComponent<Rigidbody>();
    }

    private void PlaySoundScaled(float collisionForce)
    {
        // Play the audio based on the collision force
        if (collisionForce > strongImpactThreshold) {
            // Harsh collision
            // Play the audio from the start of the clip (with the bell 'clink')
            audioSource.time = audioClipStartTime;
        } else {
            // Scale the audio proportional to collision force
            float impactEffect = collisionForce * impactEffectFactor;
            // Limit the effect based on current playback and parameter limits
            impactEffect = Mathf.Clamp(impactEffect, minimumImpactEffect, audioSource.time - audioClipStartTime);
            impactEffect = Mathf.Clamp(impactEffect, 0, audioSource.time);
            
            // Make sure impactEffect doesn't violate audioClipStartTime
            if (audioSource.time - impactEffect < audioClipStartTime)
            {
                audioSource.time = audioClipStartTime;
            } else
            {
                audioSource.time = audioSource.time - impactEffect;
            }

            

            // enforce minimum impact playback time
            if (audioSource.time > minimumImpactClipStartTime) {
                audioSource.time = minimumImpactClipStartTime;
            }
        }
        Debug.Log("Start time: " + audioClipStartTime + "\n" +
            "New bell time: " + audioSource.time);

        // Play the audio
        audioSource.Play();
    }

    private void OnCollisionEnter(Collision collision)
    {
        float collisionForce = collision.impulse.magnitude;
        //Debug.Log("Collision detected with impulse: " + collision.impulse.ToString());

        PlaySoundScaled(collisionForce);
    }

    public void Plink(float impulse)
    {
        // Force a collision with nothing
        Vector3 swingDirection = transform.position - transform.parent.position;

        rb.AddForce(swingDirection * impulse, ForceMode.Impulse);

        
        
        //rb.AddRelativeForce(Vector3.forward)

        
        PlaySoundScaled(impulse);
    }


}
