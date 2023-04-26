using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class GameLoopManager : MonoBehaviour
{
    public static GameLoopManager instance;
    
    public static PatternManager patternManager;
    public static InteractionPool interactionPool;
    public static CombatManager combatManager;
    public static ExplorationManager explorationManager;

    public LevelData levelData;
    public LevelChunk currentChunk;
    
    [Header("Interaction")]
    public Transform midSpawnPoint;
    public Transform interactionParent;
    public InteractionDetector detector;
    public GameObject interactionPrefab;

    [Header("Temp")] 
    public ParticleSystem bpmVisual;
    
    public int tickCount;

    private void Awake()
    {
        if (instance == null) instance = this;

        interactionPool = new InteractionPool(interactionParent, interactionPrefab);
        patternManager = new PatternManager();
        combatManager = new CombatManager();
        explorationManager = new ExplorationManager();

        GameManager.OnTick += (() => tickCount++);
        GameManager.OnTick += () => bpmVisual.Play();
    }
    
    public void InitLevel()
    {
        GameManager.gameState.SwitchTimeState(Enums.TimeState.Play);
        
        PlayerManager.instance.SetPlayer();
        PlayPattern();
    }

    private void PlayPattern()
    {
        tickCount = 0;
        
        combatManager.InitCombat(levelData.enemy);
    }

    public void CheckForNextPattern()
    {
        PlayPattern();
    }
    
    public void EndLevel()
    {
        GameManager.gameState.SwitchTimeState(Enums.TimeState.Pause);
       
        UIManager.instance.endLevel.DrawPanel();
        
        patternManager.EndPattern();
    }
    
    public void Restart()
    {
        interactionPool.DisableAllInteractions();

        StreakManager.ResetStreak();
        ScoreManager.ResetScore();
        
        GameManager.gameState.SwitchTimeState(Enums.TimeState.Play);

        InitLevel();
    }
}
