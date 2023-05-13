using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private int scoreOk = 5;
    [SerializeField] private int scoreGood = 10;
    [SerializeField] private int scorePerfect = 20;
    
    private static ScoreManager instance;
    private float score;

    private void Awake()
    {
        if (instance == null) instance = this;
        ResetScore();
    }

    public static void AddScore(InteractionSuccess  interactionSuccess)
    {
        instance.score += interactionSuccess == InteractionSuccess.Ok ? instance.scoreOk : 
                           interactionSuccess == InteractionSuccess.Good ? instance.scoreGood : instance.scorePerfect;
    }
    
    public static float GetScore()
    {
        return instance.score * StreakManager.GetMultiplier() * 
               GameManager.instance.currentCharacterInfos.playerStats.strength;
    }

    public static void ResetScore()
    {
        instance.score = 0;
    }
}