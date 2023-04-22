using System;
using Code.Interface;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

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

    public bool justPerfectEnabled = false;
    [Header("DEBUG")] public Image healthFill;
    public TMP_Text healthTxt;
    public Image CDPowerImage;

    public static Action<InteractionSuccess> onInteractionSuccess;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        playerStats = GameManager.instance.currentCharacterInfos.playerStats;
        isMoving = false;
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
    private int index;

    public void HurtEnemy()
    {
        EnemyManager.instance.GetHurt(currentStats.damage);
    }

    public void MovePlayerTo(Vector3 pos, LevelRoadManager.RoadStep.StepAction stepAction = LevelRoadManager.RoadStep.StepAction.NONE)
    {
        nextAction = stepAction;
        targetPosition = pos;
        previousPosition = transform.position;
        isMoving = true;

        if (stepAction == LevelRoadManager.RoadStep.StepAction.ENNEMY)
        {
            animator.SetTrigger(index % 2 == 0 ? "AttackLeft" : "AttackRight");
        }
        else
        {       
            animator.SetTrigger(index % 2 == 0 ? "StepLeft" : "StepRight");
        }
        
        index++;

        runningStep = 0;
    }

    void Move()
    {
        runningStep += runningSpeed * Time.deltaTime;
        runningStep = Mathf.Clamp(runningStep, 0, 1);

        Vector3 oldPos = transform.position;

        transform.position = Vector3.Lerp(previousPosition, targetPosition, runningStep);

        float newPos = transform.position.z - oldPos.z;
        /*
        Transform[] enemies = TileManager.GetCurrentTile().ToArray();
        foreach (Transform enemy in enemies)
            enemy.position += new Vector3(0, 0, newPos);
        */
        if (runningStep >= 1)
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
        currentExperience += amount;
        CheckForLevelUp();
    }

    void CheckForLevelUp()
    {
        if (level >= playerLevelingData.experienceTable.Length - 1) return;

        if (currentExperience >= playerLevelingData.experienceTable[level])
        {
            float rest = currentExperience - playerLevelingData.experienceTable[level];
            level++;
            currentExperience = rest;
        }
    }

    public void OnInteractionSuccess(InteractionSuccess interactionSuccess)
    {
        StreakManager.AddStreak();
        switch (interactionSuccess)
        {
            case InteractionSuccess.Ok:
                UIManager.instance.announcer.Announce("Ok", Color.red);

                if (GameManager.instance.gameState.IsLevelExploration())
                {
                    LevelManager.instance.roadManager.CheckStepsToTarget(5);
                    ScoreManager.AddScore(5);
                }
                else
                {
                   HurtEnemy();
                }

                break;

            case InteractionSuccess.Good:
                UIManager.instance.announcer.Announce("Good", Color.blue);

                if (GameManager.instance.gameState.IsLevelExploration())
                {
                    LevelManager.instance.roadManager.CheckStepsToTarget(10);
                    ScoreManager.AddScore(10);
                }
                else
                {
                    HurtEnemy();
                }

                break;

            case InteractionSuccess.Perfect:
                UIManager.instance.announcer.Announce("Perfect", Color.green);

                if (GameManager.instance.gameState.IsLevelExploration())
                {
                    LevelManager.instance.roadManager.CheckStepsToTarget(20);
                    StreakManager.AddStreak();
                    ScoreManager.AddScore(20);
                }
                else
                {
                    HurtEnemy();
                }
                break;
        }

        distanceReached = ScoreManager.GetScore();
        UIManager.instance.score.SetScore(distanceReached);
    }

    public void TakeDamage(float amount)
    {
        currentStats.hp -= amount;

        SetUIHealth();

        if (currentStats.hp <= 0)
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