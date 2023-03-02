using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossPattern : MonoBehaviour
{
    public float MaxHealth = 500f;
    [HideInInspector] public float CurrentHealth = -999f;
    public Pattern[] Patterns;
    
    public bool success;
    
    public void LaunchPattern()
    {
        PatternManager.Instance.StartPattern(Patterns[Random.Range(0, Patterns.Length)]);
        PatternManager.OnPatternEnd += OnPatternEnd;
        InputManager.OnFailedTouchInteraction += OnFailedTouchInteraction;
        success = true;
    }

    private void OnFailedTouchInteraction()
    {
        Debug.Log("Failed");
        success = false;
    }


    private void OnPatternEnd()
    {
        InputManager.OnFailedTouchInteraction -= OnFailedTouchInteraction;
        PatternManager.OnPatternEnd -= OnPatternEnd;
        if (!success)
            BossFightManager.Instance.FailedToMiss();
        else
        {
            BossFightManager.Instance.BossTakeDamage();
        }
    }

    private void OnEnable()
    {
        CurrentHealth = MaxHealth;
    }
}
