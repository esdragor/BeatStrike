using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public float distanceReached;
    private PlayerStats playerStats;
    public PlayerStats currentStats;
    public Animator animator;
    
    public PlayerLevelingData playerLevelingData;
    public float currentExperience;
    public int level = 1;

    public float runTime;
    private bool isRunning;

    [Header("DEBUG")] 
    public Image healthFill;
    public TMP_Text healthTxt;
    
    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        playerStats = GameManager.instance.currentCharacterInfos.playerStats;
        
        SetPlayer();
    }

    public void SetPlayer()
    {
        currentStats = new PlayerStats(playerStats.hp, playerStats.speed, playerStats.experienceFactor, playerStats.damage);
        distanceReached = 0;
        SetUIHealth();
        UIManager.instance.score.SetScore(distanceReached);
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
                UIManager.instance.announcer.Announce("Ok", Color.red);

                if (GameManager.instance.gameState.IsLevelExploration())
                {
                    animator.SetBool("IsRunning", true);
                    LevelManager.instance.MoveWorld(5, currentStats.speed, animator);
                    distanceReached += 5;
                }
                break;
            
            case InteractionSuccess.Good:
                UIManager.instance.announcer.Announce("Good", Color.blue);

                if (GameManager.instance.gameState.IsLevelExploration())
                {
                    animator.SetBool("IsRunning", true);
                    LevelManager.instance.MoveWorld(10, currentStats.speed, animator);
                    distanceReached += 10;
                }
                break;
            
            case InteractionSuccess.Perfect:
                UIManager.instance.announcer.Announce("Perfect", Color.green);
                
                if (GameManager.instance.gameState.IsLevelExploration())
                {
                    isRunning = true;
                    animator.SetBool("IsRunning", true);
                    LevelManager.instance.MoveWorld(15, currentStats.speed, animator);
                    distanceReached += 15;
                }
                break;
        }
        
        UIManager.instance.score.SetScore(distanceReached);
    }

    public void TakeDamage(float amount)
    {
        currentStats.hp -= amount;

        SetUIHealth();
        
        if (currentStats.hp  <= 0)
        {
            UIManager.instance.announcer.Announce("You died !", Color.black);
           LevelManager.instance.EndLevel();
        }
    }

    void SetUIHealth()
    {
        UIManager.instance.hud.playerHealth.SetHealth(currentStats.hp, playerStats.hp);
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