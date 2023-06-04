using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager instance;

    [SerializeField] private int scoreOk = 5;
    [SerializeField] private int scoreGood = 10;
    [SerializeField] private int scorePerfect = 20;
    [SerializeField] private float modifierEasy = 0.75f;
    [SerializeField] private float modifierMedium = 1.0f;
    [SerializeField] private float modifierHard = 1.25f;
    private float multiplierDifficulty = 1f;
    
    private float score;

    private void Awake()
    {
        if (instance == null) instance = this;
        ResetScore();
    }

    public static void AddScore(InteractionSuccess  interactionSuccess)
    {
        instance.score += (interactionSuccess == InteractionSuccess.Ok ? instance.scoreOk : 
                           interactionSuccess == InteractionSuccess.Good ? instance.scoreGood : instance.scorePerfect) * instance.multiplierDifficulty * StreakManager.GetMultiplier();
    }
    
    public static float GetScore()
    {
        return instance.score;
    }
    
    public static void ModifierDifficulty(byte multiplier)
    {
        switch (multiplier)
        {
            case 0:
                instance.multiplierDifficulty = instance.modifierEasy;
                break;
            case 1:
                instance.multiplierDifficulty = instance.modifierMedium;
                break;
            case 2:
                instance.multiplierDifficulty = instance.modifierHard;
                break;
        }
    }

    public static void ResetScore()
    {
        instance.score = 0;
    }
}