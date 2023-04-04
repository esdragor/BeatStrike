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
    [Header("DEBUG")] 
    public Image healthFill;
    public TMP_Text healthTxt;
    public Image CDPowerImage;

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
        currentStats = new PlayerStats(playerStats);
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
        GameManager.instance.currentCharacterInfos.power.ModifyCooldown(interactionSuccess);
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
    
    public PlayerStats(PlayerStats other)
    {
        hp = other.hp;
        speed = other.speed;
        experienceFactor = other.experienceFactor;
        damage = other.damage;
        competenceDuration = other.competenceDuration;
        critRate = other.critRate;
        critTolerance = other.critTolerance;
    }

    private float ModVal( in float val, float amount, float min, float max)
    {
        float NewVal = val;
        NewVal += amount;
        if (val < min) NewVal = min;
        if (val > max) NewVal = max;
        return NewVal;
    }
    
    private float SetVal( in float val, float min, float max)
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
                hp = ModVal(hp, value, minHp, maxHp);
                break;
            case StatsType.Speed:
                speed = ModVal(speed, value, minSpeed, maxSpeed);
                break;
            case StatsType.ExperienceFactor:
                experienceFactor = ModVal(experienceFactor, value, minExperienceFactor, maxExperienceFactor);
                break;
            case StatsType.Damage:
                damage = ModVal(damage, value, minDamage, maxDamage);
                break;
            case StatsType.CompetenceDuration:
                competenceDuration = ModVal(competenceDuration, value, minCompetenceDuration, maxCompetenceDuration);
                break;
            case StatsType.CritRate:
                critRate = ModVal(critRate, value, minCritRate, maxCritRate);
                break;
            case StatsType.CritTolerance:
                critTolerance = ModVal(critTolerance, value, minCritTolerance, maxCritTolerance);
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
                break;
            case StatsType.Speed:
                speed = SetVal(value, minSpeed, maxSpeed);
                break;
            case StatsType.ExperienceFactor:
                experienceFactor = SetVal(value, minExperienceFactor, maxExperienceFactor);
                break;
            case StatsType.Damage:
                damage = SetVal(value, minDamage, maxDamage);
                break;
            case StatsType.CompetenceDuration:
                competenceDuration = SetVal(value, minCompetenceDuration, maxCompetenceDuration);
                break;
            case StatsType.CritRate:
                critRate = SetVal(value, minCritRate, maxCritRate);
                break;
            case StatsType.CritTolerance:
                critTolerance = SetVal(value, minCritTolerance, maxCritTolerance);
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
    Speed,
    ExperienceFactor,
    Damage,
    CompetenceDuration,
    CritRate,
    CritTolerance
}