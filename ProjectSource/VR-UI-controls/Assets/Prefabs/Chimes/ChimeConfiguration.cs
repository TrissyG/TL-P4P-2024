using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ChimeConfiguration : MonoBehaviour
{
    public GameObject bellPrefab;

    // Disc from which bells hang
    public GameObject bellsBase;

    

    [Range(0, 12)]
    public int numberOfBells = 6;

    public float bellsSpacingFromCentre = 0.5f;
    public float bellsHangingDistance = 0.5f;

    public List<AudioClip> bellSounds;

    // should only be available to other scripts
    [System.NonSerialized]
    public List<GameObject> bells;

    // The group which all chime sounds output to.
    public AudioMixerGroup audioMixerGroup;

    private void Start()
    {
        // init variables
        bells = new List<GameObject>();


        // Generate bell locations in a circle
        List<Vector3> bellPositions = CreateBellPositions();

        int index = 0;
        foreach (Vector3 bellPosition in bellPositions) {
            // Instantiate bell
            GameObject bell = Instantiate(bellPrefab, bellPosition, transform.rotation, gameObject.transform);
            bells.Add(bell);

            // Set the bell's connected body to be the base
            ConfigurableJoint joint = bell.GetComponent<ConfigurableJoint>();
            joint.connectedBody = bellsBase.GetComponent<Rigidbody>();

            // Set the anchor point (above the bell's position, based on the hanging distance and the size of the bell
            float distanceFromAnchorPoint = bellsHangingDistance + 1; // Half the height of the bell is 1 unit before scaling!
            joint.anchor = Vector3.up * distanceFromAnchorPoint;

            // Set the bell's sound, duplicating sounds if neccessary.
            bell.GetComponent<AudioSource>().clip = bellSounds[index];
            index = (index + 1) % bellSounds.Count;

            // route bell sounds through audioMixer for volume control.
            if (audioMixerGroup != null) {
                bell.GetComponent<AudioSource>().outputAudioMixerGroup = audioMixerGroup;
            }

            
        }

        
    }

    private void OnDrawGizmos()
    {
        // Visualise bell positions
        List<Vector3> bellPositions = CreateBellPositions();
        foreach (Vector3 bellPosition in bellPositions) {
            Gizmos.color = Color.white;
            Gizmos.DrawWireMesh(bellPrefab.GetComponent<MeshFilter>().sharedMesh, bellPosition, bellPrefab.transform.rotation, bellPrefab.transform.localScale);
        }
        
    }

    private List<Vector3> CreateBellPositions()
    {
        // Evenly space points in the shape of a circle on the x,z plane
        // Returns a list of normalised vectors

        List<Vector3> bellPositions = new List<Vector3>();

        for (int i = 0; i < numberOfBells; i++) {
            float angle = 2 * Mathf.PI * ((float)i / (numberOfBells));
            Vector3 bellPosition = new Vector3(Mathf.Sin(angle) * bellsSpacingFromCentre, 0, Mathf.Cos(angle) * bellsSpacingFromCentre);
            bellPosition += bellsBase.transform.position + Vector3.down * bellsHangingDistance;
            bellPositions.Add(bellPosition);
        }

        return bellPositions;
    }



}
