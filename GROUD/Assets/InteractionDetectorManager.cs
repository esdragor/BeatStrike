using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionDetectorManager : MonoBehaviour
{
    public InteractionDetector okDetector;
    public InteractionDetector goodDetector;
    public InteractionDetector perfectDetector;
    
    private void OnEnable()
    {
        okDetector.OnInteractionAdded += SetOkInteractionGroup;
        goodDetector.OnInteractionAdded += SetGoodInteractionGroup;
        perfectDetector.OnInteractionAdded += SetPerfectInteractionGroup;
    }

    private void OnDisable()
    {
        okDetector.OnInteractionAdded -= SetOkInteractionGroup;
        goodDetector.OnInteractionAdded -= SetGoodInteractionGroup;
        perfectDetector.OnInteractionAdded -= SetPerfectInteractionGroup;
    }

    private void SetOkInteractionGroup(InteractionComponent it)
    {
       it.SetSuccess(InteractionSuccess.Ok);
    }
    
    private void SetGoodInteractionGroup(InteractionComponent it)
    {
        it.SetSuccess(InteractionSuccess.Good);
    }
    
    private void SetPerfectInteractionGroup(InteractionComponent it)
    {
        it.SetSuccess(InteractionSuccess.Perfect);
    }

}
