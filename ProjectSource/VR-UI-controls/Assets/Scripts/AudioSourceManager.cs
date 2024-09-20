using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using System.Collections.Generic;

namespace AudioSourceManagement
{
    public class AudioSourceManager : MonoBehaviour
    {
        public GameObject radioPolygon; // the rendered radio object 'Radio', around which the sound object will be placed
        public GameObject radio; // the 'Audio Source' GameObject child of the rendered object, to be offset
        public AudioSource audioSource;
        private Polar polarOffset; // (radius, inclination, azimuth) of the sound object relative to the radioPolygon
        private List<PresetData> presetDataList;
        private void Awake()
        {
            this.polarOffset = new Polar(0f, 0f, 0f);
             presetDataList = DataReadingManager.ReadCSV("vr_locationing_combinations");
        }


        // Constructor to initialize the AudioSourceManager with required objects
        public AudioSourceManager(GameObject radioPolygon, GameObject radio, AudioSource audioSource)
        {
            this.radioPolygon = radioPolygon;
            this.radio = radio;
            this.audioSource = audioSource;
            Awake();

            // Ensure soundObject and parentObject are set before updating position
            if (this.radioPolygon != null && this.radio != null)
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
            if (radioPolygon == null || radio == null)
            {
                Debug.LogWarning("radioPolygon or radio is not assigned.");
                return;
            }
            // Debug.Log("Setting " + polarOffset + " spherical coordinates to: " + radius + ", " + inclination + ", " + azimuth);
            this.polarOffset.radius = radius;
            this.polarOffset.inclination = inclination;
            this.polarOffset.azimuth = azimuth;
            UpdateSoundObjectPosition();
        }

        public void setOffsetPreset(int index)
        {
            PresetData preset = presetDataList.Find(p => p.Index == index);
            if (preset != null)
            {
                SetSphericalCoordinates(preset.Radius, preset.Inclination, 0);
                Debug.Log("Preset: " + index + " Radius: " + preset.Radius + " Inclination: " + preset.Inclination);
            }
            else
            {
                Debug.LogWarning("Invalid index for offset preset.");
            }
        }

        private void UpdateSoundObjectPosition()
        {
            if (radioPolygon == null || radio == null)
            {
                Debug.LogWarning("radioPolygon or radio is not assigned.");
                return;
            }

            Vector3 offset = polarOffset.ToCartesian();
            Vector3 rotatedOffset = radioPolygon.transform.TransformDirection(offset);
            radio.transform.position = radioPolygon.transform.position + rotatedOffset;
        }
    }

    public class Polar
    {
        public float radius, inclination, azimuth;

        public static Vector3 ToCartesian(float radius, float inclination, float azimuth)
        {
            float x = radius * Mathf.Sin(inclination * Mathf.Deg2Rad) * Mathf.Cos(azimuth * Mathf.Deg2Rad);
            float y = radius * Mathf.Cos(inclination * Mathf.Deg2Rad);
            float z = radius * Mathf.Sin(inclination * Mathf.Deg2Rad) * Mathf.Sin(azimuth * Mathf.Deg2Rad);
            return new Vector3(x, y, z);
        }

        public Polar(float radius, float inclination, float azimuth)
        {
            this.radius = radius;
            this.inclination = inclination;
            this.azimuth = azimuth;
        }

        public Vector3 ToCartesian()
        {
            return Polar.ToCartesian(radius, inclination, azimuth);
        }
    }
}

