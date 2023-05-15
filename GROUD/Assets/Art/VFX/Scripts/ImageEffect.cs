using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteInEditMode]
public class ImageEffect : MonoBehaviour
{
    public AnimationCurve[] aCurve;
    [Range(0.0f,1.0f)]
    public float lerpValue;
    public Material[] mat;
    public bool isActive;
    [Range(0.0f,5.0f)]
    public float effectSpeed;
    public ParticleSystem[] pSystem;
    private int _propID;
    public int effID = 0;
    private bool _yes = true;
    private void Start()
    {
        if (pSystem == null) _yes = false;
    }

    private void Update()
    {
        if (pSystem[effID].isPlaying && _yes) isActive = true;
        else
        {
            isActive = false;
            lerpValue = 0;
        }
        mat[effID].SetFloat("_LerpValue", lerpValue);
        
        if (isActive)
        {
            lerpValue = aCurve[effID].Evaluate(pSystem[effID].time/pSystem[effID].main.duration);
        }
    }
    
    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, dest, mat[effID]);
    }
}
