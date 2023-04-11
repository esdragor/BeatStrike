using System;
using UnityEngine;
using Utilities;

public class RunnerManager : MonoBehaviour
{
    public PatternSO[] patterns;
    
    private int currentPatternIndex = 0;

    private void Awake()
    {
        PatternManager.OnPatternEnd += OnPatternEnd;
    }

    public void Begin()
    {
        PatternManager.Instance.StartPattern(patterns[currentPatternIndex]);
    }

    private void OnPatternEnd()
    {
    }
}