using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public float distanceReached;
    private Vector3 previousPosition;
    private Vector3 targetPosition;
    public float runningSpeed;
    private PlayerStats playerStats;
    public PlayerStats currentStats;
    public Animator animator;
    
    public PlayerLevelingData playerLevelingData;
    public float currentExperience;
    public int level = 1;

    private bool isMoving;

    public bool powerIsRunning = false;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        playerStats = GameManager.instance.currentCharacterInfos.playerStats;
        
        SetPlayer();
    }

    private void Update()
    {
        if (isMoving)
        {
            Move();
        }
    }

    private float runningStep;
    private LevelRoadManager.RoadStep.StepAction nextAction;
    public void MovePlayerTo(Vector3 pos, LevelRoadManager.RoadStep.StepAction stepAction = LevelRoadManager.RoadStep.StepAction.NONE)
    {
        Debug.Log($"Player Move");
        nextAction = stepAction;
        targetPosition = pos;
        previousPosition = transform.position;
        isMoving = true;
    }
    
    void Move()
    {
        runningStep += runningSpeed * Time.deltaTime;
        runningStep = Mathf.Clamp(runningSpeed, 0, 1);
        
        transform.position = Vector3.Lerp(previousPosition, targetPosition, runningStep);

        if (runningSpeed >= 1)
        {
            Debug.Log($"Player Reach End");
            isMoving = false;
            switch (nextAction)
            {
                case LevelRoadManager.RoadStep.StepAction.ENNEMY:
                    break;
                
                case LevelRoadManager.RoadStep.StepAction.END:
                    LevelManager.instance.EndLevel();
                    break;
            }
        }
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
                    LevelManager.instance.roadManager.CheckStepsToTarget((int)currentStats.speed + 5);
                    
                }
                break;
            
            case InteractionSuccess.Good:
                UIManager.instance.announcer.Announce("Good", Color.blue);

                if (GameManager.instance.gameState.IsLevelExploration())
                {
                    LevelManager.instance.roadManager.CheckStepsToTarget((int)currentStats.speed + 10);

                }
                break;
            
            case InteractionSuccess.Perfect:
                UIManager.instance.announcer.Announce("Perfect", Color.green);
                
                if (GameManager.instance.gameState.IsLevelExploration())
                {
                    LevelManager.instance.roadManager.CheckStepsToTarget((int)currentStats.speed + 20);
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
    [Header("Stats")]
    public float hp;
    public float competenceDuration;
    public float damage;
    public float speed;
    public float critRate;
    public float critTolerance;
    
    public float experienceFactor;
    
    [Header("Stats Bornes")]
    public float minHp;
    public float maxHp;
    public float minSpeed;
    public float maxSpeed;
    public float minExperienceFactor;
    public float maxExperienceFactor;
    public float minDamage;
    public float maxDamage;
    public float minCompetenceDuration;
    public float maxCompetenceDuration;
    public float minCritRate;
    public float maxCritRate;
    public float minCritTolerance;
    public float maxCritTolerance;

    public PlayerStats(float _hp, float _speed, float _experienceFactor, float _damage, float _competenceDuration, float _critRate, float _critTolerance)
    {
        hp = _hp;
        speed = _speed;
        experienceFactor = _experienceFactor;
        damage = _damage;
        competenceDuration = _competenceDuration;
        critRate = _critRate;
        critTolerance = _critTolerance;
    }

    public void SetHp(float amount)
    {
        if (amount < minHp) amount = minHp;
        if (amount > maxHp) amount = maxHp;
        hp = amount;
    }
    public void SetSpeed(float amount)
    {
        if (amount < minSpeed) amount = minSpeed;
        if (amount > maxSpeed) amount = maxSpeed;
        speed = amount;
    }
    public void SetTolerance(float amount) => critTolerance = amount;
    public void SetExperienceFactor(float amount)
    {
        if (amount < minExperienceFactor) amount = minExperienceFactor;
        if (amount > maxExperienceFactor) amount = maxExperienceFactor;
        experienceFactor = amount;
    }
    public void SetDamage(float amount)
    {
        if (amount < minDamage) amount = minDamage;
        if (amount > maxDamage) amount = maxDamage;
        damage = amount;
    }
    public void SetCompetenceDuration(float amount)
    {
        if (amount < minCompetenceDuration) amount = minCompetenceDuration;
        if (amount > maxCompetenceDuration) amount = maxCompetenceDuration;
        competenceDuration = amount;
    }
    public void SetCritRate(float amount)
    {
        if (amount < minCritRate) amount = minCritRate;
        if (amount > maxCritRate) amount = maxCritRate;
        critRate = amount;
    }
    public void SetCritTolerance(float amount)
    {
        if (amount < minCritTolerance) amount = minCritTolerance;
        if (amount > maxCritTolerance) amount = maxCritTolerance;
        critTolerance = amount;
    }
}

public enum InteractionSuccess
{
    Ok,
    Good,
    Perfect
}