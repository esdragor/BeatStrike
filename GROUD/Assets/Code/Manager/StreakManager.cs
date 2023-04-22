using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreakManager : MonoBehaviour
{
    [SerializeField] private int nbNeedToMultiply = 4;
    
    private static StreakManager instance;
    
    private float streak = 0;
    
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UIManager.instance.streak.Enable();
        ResetStreak();
    }

    public static void AddStreak()
    {
        instance.streak++;
        UIManager.instance.streak.UpdateStreakAndMultiplier();
    }
    
    public static float GetStreak()
    {
        return instance.streak;
    }
    
    public static void ResetStreak()
    {
        instance.streak = 0;
    }
    
    public static int GetNbNeedToMultiply()
    {
        return instance.nbNeedToMultiply;
    }
    
    public static void RemoveStreak()
    {
        instance.streak--;
        UIManager.instance.streak.UpdateStreakAndMultiplier();
    }

    public static int GetMultiplier()
    {
        return 1 + (int)(instance.streak / instance.nbNeedToMultiply);
    }
    
    public static int GetCurrentLoadBar()
    {
        return (int)(instance.streak % instance.nbNeedToMultiply);
    }
}
