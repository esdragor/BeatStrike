using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPattern : MonoBehaviour
{
    public float MaxHealth = 500f;
    [HideInInspector] public float CurrentHealth = -999f;
    public Pattern[] Patterns;
    
    public bool success;
    
    public void LaunchPattern(int index)
    {
        PatternManager.Instance.StartPattern(Patterns[index], null, Vector3.zero);
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
        if (!success)
            BossFightManager.Instance.FailedToMiss();
        BossFightManager.Instance.EndBossTurn();
        InputManager.OnFailedTouchInteraction -= OnFailedTouchInteraction;
        PatternManager.OnPatternEnd -= OnPatternEnd;
    }

    private void OnEnable()
    {
        CurrentHealth = MaxHealth;
    }
}
