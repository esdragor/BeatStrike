using System;
using Code.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public static Action<InteractionSuccess> onInteractionSuccess;
    public static Action<InteractionSuccess> onComboSuccess;

    public float distanceReached;
    public int MaxHP => (int)GameManager.instance.currentCharacterInfos.playerStats.hp;
    public Animator animator;

    [Header("DEBUG")] public UI_PlayerHealth healthFill;
    public TMP_Text healthTxt;
    public Image CDPowerImage;


    private Vector3 previousPosition;
    private Vector3 targetPosition;
    private PowerSO currentPower;
    private int currentHP;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public PowerSO GetCurrentPower()
    {
        return currentPower;
    }

    private void Start()
    {
        SetPlayer();
        PowerManager.AssignNewPower();
    }

    public void MovePlayerTo(Vector3 position)
    {
        Vector3 nextPosition = new Vector3(transform.position.x, transform.position.y, position.z);
        transform.position = position;
    }

    public void HurtEnemy(int damage = 1)
    {
        GameLoopManager.combatManager.DealDamage(damage);
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

        MovePlayerTo(GameLoopManager.instance.currentChunk.combatPos.position);
    }

    private void SetInputComponent(Enums.InteractionType interactionType, ScreenListener.SwipeDirection dataSwipeDirection)
    {
        switch (interactionType)
        {
            case Enums.InteractionType.Attack:
                InteractionSuccess success = currentPower.power.Execute(dataSwipeDirection);
                if (success == InteractionSuccess.Ok)
                    onComboSuccess?.Invoke(InteractionSuccess.Ok);
                else 
                    PowerManager.AssignNewPower();

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


    public void OnInteractionSuccess(InteractionSuccess interactionSuccess, Enums.InteractionType interactionType, ScreenListener.SwipeDirection dataSwipeDirection)
    {
        StreakManager.AddStreak();


        ScoreManager.AddScore(interactionSuccess);
        
        if (GameManager.gameState.IsLevelExploration())
        {
            SetInputComponent(interactionType, dataSwipeDirection);
        }

        distanceReached = ScoreManager.GetScore();
        UIManager.instance.score.SetScore((int)distanceReached);
    }

    public void SetPower(PowerSO newPower)
    {
        currentPower = newPower;
    }
}

public enum InteractionSuccess
{
    Ok,
    Good,
    Perfect,
    Fail,
}

public enum StatsType
{
    Hp,
    Intelligence,
    Strength
}