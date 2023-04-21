using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager instance;
    private float score;

    private void Awake()
    {
        instance = this;
    }

    public static void AddScore(float value)
    {
        instance.score += value;
    }
    
    public static float GetScore()
    {
        return instance.score * StreakManager.GetMultiplier();
    }
}