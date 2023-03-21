using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public PlayerStats playerStats;
    public PlayerStats currentStats;
    public Animator animator;
    
    public PlayerLevelingData playerLevelingData;
    public float currentExperience;
    public int level = 1;

    public float runTime;
    private bool isRunning;
    private void Awake()
    {
        if (instance == null) instance = this;
        SetPlayer();
    }

    private void Update()
    {
        if (isRunning)
        {
            if (runTime > 0)
            {
                runTime -= Time.deltaTime * currentStats.speed;
            }
            else
            {
                animator.SetBool("IsRunning", false);
                isRunning = false;
                runTime = 0;
            }
        }
    }

    public void SetPlayer()
    {
        currentStats = new PlayerStats(playerStats.hp, playerStats.speed, playerStats.experienceFactor, playerStats.damage);
    }

    public void AddExperience(float amount)
    {
        currentExperience += amount + (amount * currentStats.experienceFactor);
        CheckForLevelUp();
    }

    void CheckForLevelUp()
    {
        if(level >= playerLevelingData.experienceTable.Length -1) return;
        
        if (currentExperience >= playerLevelingData.experienceTable[level])
        {
            float rest = currentExperience - playerLevelingData.experienceTable[level];
            level++;
            currentExperience = rest;
        }
    }

    public void OnInteractionSuccess(InteractionSuccess interactionSuccess)
    {
        switch (interactionSuccess)
        {
            case InteractionSuccess.Ok:
                if (GameManager.instance.gameState.IsLevelExploration())
                {
                    isRunning = true;
                    animator.SetBool("IsRunning", true);
                    runTime = currentStats.speed * 1;
                    LevelManager.instance.MoveWorld(runTime, currentStats.speed, animator);
                }
                else
                {
                    
                }
                break;
            
            case InteractionSuccess.Good:
                if (GameManager.instance.gameState.IsLevelExploration())
                {
                    isRunning = true;
                    animator.SetBool("IsRunning", true);
                    runTime += currentStats.speed * 2;
                }
                else
                {
                    
                }
                break;
            
            case InteractionSuccess.Perfect:
                if (GameManager.instance.gameState.IsLevelExploration())
                {
                    isRunning = true;
                    animator.SetBool("IsRunning", true);
                    runTime += currentStats.speed * 3;
                }
                else
                {
                    
                }
                break;
        }
    }

    public void TakeDamage(float amount)
    {
        currentStats.hp -= amount;

        if (currentStats.hp  <= 0)
        {
            Debug.Log("Player is dead");
        }
    }
}

[Serializable] public class PlayerStats
{
    public float hp;
    public float speed;
    public float tolerance;
    public float experienceFactor;
    public float damage;
    
    public PlayerStats(float hp, float speed, float experienceFactor, float damage)
    {
        this.hp = hp;
        this.speed = speed;
        this.experienceFactor = experienceFactor;
        this.damage = damage;
    }

    public void SetHp(float amount) => hp = amount;
    public void SetSpeed(float amount) => speed = amount;
    public void SetTolerance(float amount) => tolerance = amount;
    public void SetExperienceFactor(float amount) => experienceFactor = amount;
    public void SetDamage(float amount) => damage = amount;
}

public enum InteractionSuccess
{
    Ok,
    Good,
    Perfect
}