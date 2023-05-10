using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteInEditMode]
public class ImageEffect : MonoBehaviour
{
    public AnimationCurve aCurve;
    [Range(0.0f,1.0f)]
    public float lerpValue;
    public Material mat;
    public bool isActive;
    [Range(0.0f,5.0f)]
    public float effectSpeed;
    private float _bTime = 0;
    public ParticleSystem pSystem;
    private int _propID;
    private bool _yes = true;
    private void Start()
    {
        if (pSystem == null) _yes = false;
    }

    private void FixedUpdate()
    {
        if (pSystem.isPlaying && _yes) isActive = true;
        else
        {
            isActive = false;
            _bTime = 0;
        }
        mat.SetFloat("_LerpValue", lerpValue);
        
        if (isActive)
        {
            _bTime += 0.02f;
            lerpValue = (_bTime / (1 / effectSpeed)) % 1;
            lerpValue = aCurve.Evaluate(lerpValue);
        }
    }
    
    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, dest, mat);
    }
}
