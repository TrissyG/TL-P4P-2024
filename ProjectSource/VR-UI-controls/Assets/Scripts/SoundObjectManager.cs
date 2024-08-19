using UnityEngine;

public class SoundObjectManager : MonoBehaviour
{
    public GameObject soundObject;
    public Transform parentObject;  // Parent GameObject around which the sound object will rotate
    public float distance = 5f;
    private Polar polarOffset;

    private void Start()
    {
        polarOffset = new Polar(distance, 90f, 0f);  // Initialize with some default values
        UpdateSoundObjectPosition();
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