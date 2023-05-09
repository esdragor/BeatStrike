using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteInEditMode]
public class ImageEffect : MonoBehaviour
{
    [Range(0.0f,1.0f)]
    public float lerpValue;
    public Material mat;
    public bool isActive;
    [Range(0.0f,5.0f)]
    public float effectSpeed;
    
    private void Update()
    {
        //mat.SetFloat("_LerpValue", lerpValue);
        if (isActive)
        {
            lerpValue = (Time.time / (1 / effectSpeed)) % 1;
        }
    }
    
    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, dest, mat);
    }
}
