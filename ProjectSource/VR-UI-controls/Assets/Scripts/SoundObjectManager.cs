using UnityEngine;

public class SoundObjectManager : MonoBehaviour
{   
    public string targetRadioTag = "Locationing"; // Assign the tag in the Inspector
    private GameObject targetRadio;
    public GameObject soundObject;
    public Transform parentObject;  // Parent GameObject around which the sound object will rotate
    public float distance = 5f;
    private Polar polarOffset;

    private void Start()
    {
        targetRadio = GameObject.FindGameObjectWithTag(targetRadioTag);
        if (targetRadio != null)
        {
            Debug.LogWarning("No GameObject found with the tag: " + targetRadioTag);
            return;
        }
        polarOffset = new Polar(distance, 90f, 0f);  // Initialize with some default values
        
        // Ensure soundObject and parentObject are set before updating position
        if (soundObject != null && parentObject != null)
        {
            UpdateSoundObjectPosition();
        }
        else
        {
            Debug.LogWarning("soundObject or parentObject is not assigned.");
        }
    }

    public void SetSphericalCoordinates(float radius, float inclination, float azimuth)
    {
        polarOffset.radius = radius;
        polarOffset.inclination = inclination;
        polarOffset.azimuth = azimuth;
        UpdateSoundObjectPosition();
    }

    private void UpdateSoundObjectPosition()
    {
        Vector3 offset = polarOffset.ToCartesian();
        soundObject.transform.position = parentObject.position + offset;
    }
}