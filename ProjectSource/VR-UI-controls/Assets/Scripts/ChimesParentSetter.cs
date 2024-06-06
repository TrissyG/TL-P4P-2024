using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChimesParentSetter : ParentSetter
{
    // This overrides the parent setter class to ensure that all behaviours of the chimes are reset correctly.
    public override void ResetReparenting()
    {
        reparenting = false;
        SetParentToCamera();
        gameObject.GetComponent<ChimeRandomRinger>().Disable();
        gameObject.GetComponent<ChimeAntigravity>().enabled = false;
        gameObject.GetComponent<HighlightRadio>().SetUnlock();
    }

    protected override void ForceEnterHeadlocking()
    {
        gameObject.GetComponent<ChimeRandomRinger>().enabled = true;
        gameObject.GetComponent<ChimeAntigravity>().enabled = true;
        gameObject.GetComponent<HighlightRadio>().ToggleHighlight();
    }
}
