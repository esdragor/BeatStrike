using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public float distanceReached;
    private Vector3 previousPosition;
    private Vector3 targetPosition;
    public int MaxHP = 5;

    public bool justPerfectEnabled = false;
    [Header("DEBUG")] public UI_PlayerHealth healthFill;
    public TMP_Text healthTxt;
    public Image CDPowerImage;

    public static Action<InteractionSuccess> onInteractionSuccess;
    
    private int currentHP;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        SetPlayer();
    }

    public void MovePlayerTo(Vector3 position)
    {
        Vector3 nextPosition = new Vector3(transform.position.x, transform.position.y, position.z);
        transform.position = nextPosition;
    }

    public void HurtEnemy()
    {
      GameLoopManager.combatManager.DealDamage(1);
    }

    private void OnDead()
    {
        Debug.Log("Dead");
    }
    
    public void HurtPlayer()
    {
        if (currentHP <= 0) return;
        currentHP--;
        if (!healthFill)
            healthFill = UIManager.instance.hud.playerHealth;
        
        healthFill.SetHealth(currentHP, MaxHP);
        if (currentHP <= 0)
        {
            OnDead();
        }
    }
    
    public void SetPlayer()
    {
        distanceReached = 0;
        currentHP = MaxHP;

        UIManager.instance.score.SetScore((int)distanceReached);
        UIManager.instance.hud.playerHealth.SetHealth(currentHP, MaxHP);
        
        MovePlayerTo(GameLoopManager.instance.currentChunk.levelPos.position);
    }

    private void SetInputComponent(Enums.InteractionType interactionType)
    {
        switch (interactionType)
        {
            case Enums.InteractionType.Attack:
                HurtEnemy();
                break;
            case Enums.InteractionType.Dodge:
                break;
            case Enums.InteractionType.Fake:
                 break;
            default:
                throw new ArgumentOutOfRangeException(nameof(interactionType), interactionType, null);
        }
    }


    public void OnInteractionSuccess(InteractionSuccess interactionSuccess, Enums.InteractionType interactionType)
    {
        StreakManager.AddStreak();
        int score = 0;

        switch (interactionSuccess)
        {
            case InteractionSuccess.Ok:
                score = 5;
                break;
            case InteractionSuccess.Good:
                score = 10;
                break;
            case InteractionSuccess.Perfect:
                score = 20;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(interactionSuccess), interactionSuccess, null);
        }

        ScoreManager.AddScore(score);
        if (GameManager.gameState.IsLevelExploration())
        {
            SetInputComponent(interactionType);
        }
        else
        {
            MovePlayerTo(GameLoopManager.instance.currentChunk.levelPos.position);
        }

        distanceReached = ScoreManager.GetScore();
        UIManager.instance.score.SetScore((int)distanceReached);
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