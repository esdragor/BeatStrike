using System;
using Code.AI;
using UnityEngine;
using Utilities;

public class RunnerManager : MonoBehaviour
{
    public Pattern[] patterns;

    private void Awake()
    {
        PatternManager.OnPatternEnd += OnPatternEnd;
    }

    private void Start()
    {
        PatternManager.Instance.StartPattern(patterns[0]);
    }

    private void OnPatternEnd()
    {
       GameManager.instance.StartBoss();
    }
}