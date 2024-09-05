// using UnityEngine;

// namespace AudioSourceManagement {
//     public class AudioSourceManager : MonoBehaviour
//     {   
//         public GameObject radioPolygon; // the rendered radio object 'Radio', around which the sound object will be placed
//         public GameObject radio; // the 'Audio Source' GameObject child of the rendered object, to be offset
//         public AudioSource audioSource;
//         private Polar polarOffset; // (radius, inclination, azimuth) of the sound object relative to the radioPolygon

//         private void Start()
//         {   
//             radioPolygon = GameObject.Find("Radio");
//             radio = radioPolygon.transform.Find("Audio Source").gameObject;
//             audioSource = radio.GetComponent<AudioSource>();

//             if (radio != null)
//             {
//                 Debug.LogWarning("No GameObject found for radio");
//                 return;
//             }
//             polarOffset = new Polar(0f, 0f, 0f);  // Initialize with some default values
            
//             // Ensure soundObject and parentObject are set before updating position
//             if (radioPolygon != null && radio != null)
//             {
//                 UpdateSoundObjectPosition();
//             }
//             else
//             {
//                 Debug.LogWarning("soundObject or parentObject is not assigned.");
//             }
//         }

//         public void SetSphericalCoordinates(float radius, float inclination, float azimuth)
//         {
//             polarOffset.radius = radius;
//             polarOffset.inclination = inclination;
//             polarOffset.azimuth = azimuth;
//             UpdateSoundObjectPosition();
//         }

//         private void UpdateSoundObjectPosition()
//         {
//             Vector3 offset = polarOffset.ToCartesian();
//             radio.transform.position = radioPolygon.transform.position + offset;
//         }
//     }

    
//     public class Polar : MonoBehaviour
//     {
//         public float radius, inclination, azimuth;

//         public static Vector3 ToCartesian(float radius, float inclination, float azimuth)
//         {
//             float x = radius * Mathf.Sin(inclination * Mathf.Deg2Rad) * Mathf.Cos(azimuth * Mathf.Deg2Rad);
//             float y = radius * Mathf.Cos(inclination * Mathf.Deg2Rad);
//             float z = radius * Mathf.Sin(inclination * Mathf.Deg2Rad) * Mathf.Sin(azimuth * Mathf.Deg2Rad);
//             return new Vector3(x, y, z);
//         }

//         public Polar(float radius, float inclination, float azimuth)
//         {
//             this.radius = radius;
//             this.inclination = inclination;
//             this.azimuth = azimuth;
//         }

//         public Vector3 ToCartesian()
//         {
//             return Polar.ToCartesian(radius, inclination, azimuth);
//         }
//     }
// }