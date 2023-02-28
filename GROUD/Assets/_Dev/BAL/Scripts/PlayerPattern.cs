using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class PlayerPattern : MonoBehaviour
{
    public Pattern currentPattern;
    public float Damage = 10f;
    
    private bool success;
    public void LaunchPattern()
    {
        InputManager.OnFailedTouchInteraction += OnFailedTouchInteraction;
        success = true;
        PatternManager.Instance.StartPattern(currentPattern);
        PatternManager.OnPatternEnd += OnPatternEnd;
    }

    private void OnPatternEnd()
    {
        InputManager.OnFailedTouchInteraction -= OnFailedTouchInteraction;
        PatternManager.OnPatternEnd -= OnPatternEnd;
        if (success)
        {
            BossFightManager.Instance.BossTakeDamage(Damage);
        }
        else
        {
            BossFightManager.Instance.BossTakeDamage(0);
        }
    }

    private void OnFailedTouchInteraction()
    {
        success = false;
    }
}
