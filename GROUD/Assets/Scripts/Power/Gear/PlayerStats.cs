using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerStats
{
    [Header("Stats")] public float hp;
    public float intelligence;
    public float stamina;

    [HideInInspector] public float overflowHp;
    [HideInInspector] public float overflowIntelligence;
    [HideInInspector] public float overflowStrength;

    [Header("Stats Bornes")] public float minHp;
    public float maxHp;
    public float minIntelligence;
    public float maxIntelligence;
    public float minStamina;
    public float maxStamina;

    public PlayerStats()
    {
        hp = 100;
        intelligence = 10;
        stamina = 10;
    }

    public PlayerStats(float _hp, float _intelligence, float stamina)
    {
        hp = _hp;
        intelligence = _intelligence;
        stamina = stamina;
    }

    public PlayerStats(PlayerStats other)
    {
        hp = other.hp;
        intelligence = other.intelligence;
        stamina = other.stamina;

        overflowHp = other.hp;
        overflowIntelligence = other.intelligence;
        overflowStrength = other.stamina;

        minHp = other.minHp;
        maxHp = other.maxHp;
        minIntelligence = other.minIntelligence;
        maxIntelligence = other.maxIntelligence;
        minStamina = other.minStamina;
        maxStamina = other.maxStamina;
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
                stamina = ModVal(overflowStrength, value, minStamina, maxStamina);
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
                stamina = SetVal(value, minStamina, maxStamina);
                overflowStrength = value;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
}