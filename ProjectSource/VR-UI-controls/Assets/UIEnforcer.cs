using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.UI;

public class UIEnforcer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        XRUIInputModule badComponent = gameObject.GetComponent<XRUIInputModule>();
        if (badComponent != null) {
            Debug.Log("Bad input module found during Start");
            Destroy(badComponent);
        }
    }

}
