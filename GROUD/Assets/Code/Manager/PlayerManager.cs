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