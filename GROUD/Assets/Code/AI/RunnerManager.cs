using System;
using Code.AI;
using UnityEngine;
using Utilities;

public class RunnerManager : MonoBehaviour
{
    public Pattern[] patterns;
    
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
        currentPatternIndex++;
        if (currentPatternIndex < patterns.Length)
            Begin();
        else
            GameManager.instance.StartBoss();
    }
}