using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class UI_Text : UI_Component
{
    private TMP_Text tmpText => GetComponent<TMP_Text>();
    
    [Header("Text Animation")]
    public bool randomColorOnBPM;

    private void Start()
    {
        if (randomColorOnBPM)
        {
            GameManager.OnTick += AnimationComponent;
        }
    }

    public void AnimationComponent()
    {
        tmpText.color = Random.ColorHSV();
    }
}
