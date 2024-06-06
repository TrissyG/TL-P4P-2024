using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChimeRandomRinger : MonoBehaviour
{
    public float bpm;
    public float force;

    // list of chime bells to ring
    private List<ChimeBellBehaviour> bells;

    private Coroutine ring;

    private void Start()
    {
        // Get references to all the bells
        // This requires that the ChimeConfiguration script executes first (in Project Settings)
        bells = new List<ChimeBellBehaviour>();
        foreach (GameObject bell in this.GetComponent<ChimeConfiguration>().bells) {
            bells.Add(bell.GetComponent<ChimeBellBehaviour>());
        }
    }

    private void OnEnable()
    {
        ring = StartCoroutine(Ring());
    }

    private void OnDisable()
    {
        StopCoroutine(ring);
    }

    IEnumerator Ring()
    {
        yield return null; // wait for Start()

        // Ring a random bell at the set tempo
        while (true) {
            
            bells[Random.Range(0, bells.Count)].Plink(force);

            yield return new WaitForSeconds((1 / bpm) * 60);
        }
    }

    private bool forceEnabled = false;

    public void ToggleEnabled()
    {
        // toggle but only if ForceEnable(true) has not been called.
        if (!forceEnabled) {
            if (enabled) {
                enabled = false;
            } else {
                enabled = true;
            }
        }
    }

    public void Disable()
    {
        // disable but only if ForceEnable(true) has not been called.
        if (!forceEnabled) {
            enabled = false;
        }
    }

    public void ForceEnabled(bool state)
    {
        // force enable and do not allow toggling.
        enabled = state;
        forceEnabled = state;
    }
}
