using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightRadio : MonoBehaviour
{


    //[SerializeField]
    private Color lockColor = new(0.3f, 0.3f, 0.3f, 1);
    private Color unlockColor = new(0.8f, 0.2f, 0.2f, 1);
    
    public List<Renderer> renderers;
    private List<Material> materials;
    private bool locked;

    private void Awake()
    {
        materials = new List<Material>();

        foreach (Renderer r in renderers) {
            Material material = r.material;
            materials.Add(material);
            material.DisableKeyword("_EMISSION");
        }
    }
    private void OnDisable()
    {
        foreach (Material material in materials) {
            material.EnableKeyword("_EMISSION");
        }
    }

    /* 
     * Used in Radio UX Grab interactable events when the user presses the trigger
     * if the user headlocks the item it is highlighted lockColor
     * if it is unlocked it is highlighted unlockColor
    */
    public void ToggleHighlight()
    {
        if (locked)
        {
            materials.ForEach(material => material.SetColor("_EmissionColor", unlockColor));
            locked = false;
        }
        else
        {
            materials.ForEach(material => material.SetColor("_EmissionColor", lockColor));
            locked = true;
        }
        StartCoroutine(HighlightCoroutine());
    }

    IEnumerator HighlightCoroutine()
    {
        materials.ForEach(material => material.EnableKeyword("_EMISSION"));
        yield return new WaitForSeconds(0.3f);
        materials.ForEach(material => material.DisableKeyword("_EMISSION"));
        yield return null;
    }

    public void SetUnlock()
    {
        locked = false;
    }
}
