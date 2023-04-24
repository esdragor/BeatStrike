using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public float distanceReached;
    private Vector3 previousPosition;
    private Vector3 targetPosition;
    public Animator animator;
    
    public bool justPerfectEnabled = false;
    
    [Header("DEBUG")] public Image healthFill;
    public TMP_Text healthTxt;
    public Image CDPowerImage;

    public static Action<InteractionSuccess> onInteractionSuccess;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void OnInteractionSuccess(InteractionSuccess interactionSuccess)
    {
        StreakManager.AddStreak();
        switch (interactionSuccess)
        {
            case InteractionSuccess.Ok:
                if (GameManager.gameState.IsLevelExploration())
                {
                    ScoreManager.AddScore(5);
                }
                else
                {
                    LevelManager.combatManager.DealDamage(1);
                }

                break;

            case InteractionSuccess.Good:

                if (GameManager.gameState.IsLevelExploration())
                {
                    ScoreManager.AddScore(10);
                }
                else
                {
                    LevelManager.combatManager.DealDamage(1);
                }

                break;

            case InteractionSuccess.Perfect:

                if (GameManager.gameState.IsLevelExploration())
                {
                    StreakManager.AddStreak();
                    ScoreManager.AddScore(20);
                }
                else
                {
                    LevelManager.combatManager.DealDamage(1);
                }
                break;
        }

        distanceReached = ScoreManager.GetScore();
        UIManager.instance.score.SetScore((int)distanceReached);
    }
}

[Serializable]
public class PlayerStats
{
    [Header("Stats")] public float hp;
    public float intelligence;
    public float stamina;

    public float damage = 10;
    public float critRate = 2f;

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
        hp = 0;
        intelligence = 0;
        stamina = 0;
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

public enum InteractionSuccess
{
    Ok,
    Good,
    Perfect
}

public enum StatsType
{
    Hp,
    Intelligence,
    Strength
}