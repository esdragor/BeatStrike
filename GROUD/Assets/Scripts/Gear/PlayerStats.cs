using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class PlayerStats
{
    [Header("Stats")] public float hp;
    public float intelligence;
    public float strength;

    [HideInInspector] public float overflowHp;
    [HideInInspector] public float overflowIntelligence;
    [HideInInspector] public float overflowStrength;

    [Header("Stats Bornes")] public float minHp;
    public float maxHp;
    public float minIntelligence;
    public float maxIntelligence;
    public float minStrength;
    public float maxStrength;

    public PlayerStats()
    {
        hp = 0;
        intelligence = 0;
        strength = 0;
    }

    public PlayerStats(float _hp, float _intelligence, float _strength)
    {
        hp = _hp;
        intelligence = _intelligence;
        strength = _strength;
    }

    public PlayerStats(PlayerStats other)
    {
        hp = other.hp;
        intelligence = other.intelligence;
        strength = other.strength;

        overflowHp = other.hp;
        overflowIntelligence = other.intelligence;
        overflowStrength = other.strength;
        
        minHp = other.minHp;
        maxHp = other.maxHp;
        minIntelligence = other.minIntelligence;
        maxIntelligence = other.maxIntelligence;
        minStrength = other.minStrength;
        maxStrength = other.maxStrength;
    }

    private float ModVal(in float val, float amount, float min, float max)
    {
        float NewVal = val;
        NewVal += amount;
        if (NewVal < min) NewVal = min;
        if (NewVal > max) NewVal = max;
        return NewVal;
    }

    private float ModVal(in float val, float amount)
    {
        float NewVal = val;
        NewVal += amount;
        return NewVal;
    }

    private float SetVal(in float val, float min, float max)
    {
        float NewVal = val;
        if (val < min) NewVal = min;
        if (val > max) NewVal = max;
        return NewVal;
    }

    public void ModifyValue(StatsType type, float value)
    {
        switch (type)
        {
            case StatsType.Hp:
                hp = ModVal(overflowHp, value, minHp, maxHp);
                overflowHp = ModVal(overflowHp, value);
                break;
            case StatsType.Intelligence:
                intelligence = ModVal(overflowIntelligence, value, minIntelligence, maxIntelligence);
                overflowIntelligence = ModVal(overflowIntelligence, value);
                break;
            case StatsType.Strength:
                strength = ModVal(overflowStrength, value, minStrength, maxStrength);
                overflowStrength = ModVal(overflowStrength, value);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

    public void SetValue(StatsType type, float value)
    {
        switch (type)
        {
            case StatsType.Hp:
                hp = SetVal(value, minHp, maxHp);
                overflowHp = value;
                break;
            case StatsType.Intelligence:
                intelligence = SetVal(value, minIntelligence, maxIntelligence);
                overflowIntelligence = value;
                break;
            case StatsType.Strength:
                strength = SetVal(value, minStrength, maxStrength);
                overflowStrength = value;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
    
    public void ResetStats()
    {
        hp = minHp;
        intelligence = minIntelligence;
        strength = minStrength;
        overflowHp = minHp;
        overflowIntelligence = minIntelligence;
        overflowStrength = minStrength;
    }
}