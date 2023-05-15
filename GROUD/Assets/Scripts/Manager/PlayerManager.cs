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

    public Image CDPowerImage;

    public VFXManager vfxManager;

    private Vector3 previousPosition;
    private Vector3 targetPosition;
    private PowerSO currentPower;
    private float currentHP;

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

    public void HurtEnemy(int damage)
    {
        GameLoopManager.combatManager.DealDamage(damage);
    }

    private void OnDead()
    {
        GameLoopManager.instance.EndLevel();
    }

    public void HurtPlayer(float amount)
    {
        if (currentHP <= 0) return;
        currentHP -= amount;

        vfxManager.PlaySFX("Hurt");
        
        UIManager.instance.hud.playerHealth.SetHealth(currentHP, MaxHP);
        StreakManager.RemoveStreak();
        
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

                HurtEnemy((int)GameManager.instance.currentCharacterInfos.playerStats.strength);
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

        switch (interactionSuccess)
        {
            case InteractionSuccess.Fail : vfxManager.PlaySFX("Miss", dataSwipeDirection);
                break;
            case InteractionSuccess.Ok:
                vfxManager.PlaySFX("Ok", dataSwipeDirection);
                break;
            case InteractionSuccess.Good: vfxManager.PlaySFX("Great", dataSwipeDirection);
                break;
            case InteractionSuccess.Perfect: vfxManager.PlaySFX("Perfect", dataSwipeDirection);
                break;
        }
        
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