using System.Collections;
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
    public Transform leftSpawnPoint;
    public Transform midSpawnPoint;
    public Transform rightSpawnPoint;
    public Transform interactionParent;
    public InteractionDetector detector;
    public GameObject interactionPrefab;

    [Header("Temp")] 
    public ParticleSystem bpmVisual;
    
    private int currentIndex;
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
        GameManager.gameState.SwitchLevelState(Enums.LevelState.Exploration);
        PlayerManager.instance.SetPlayer();
        PlayPattern();
    }

    private void PlayPattern()
    {
        tickCount = 0;
        
        if (GameManager.gameState.IsLevelCombat())
        {
            combatManager.InitCombat(levelData.enemy[currentIndex]);
        }
        else
        {
            explorationManager.InitExploration(levelData.corridorPattern[Random.Range(0, levelData.corridorPattern.Length)]);   
        }
    }

    public void CheckForNextPattern()
    {
        currentIndex++;
        
        GameManager.gameState.SwitchLevelState(GameManager.gameState.IsLevelCombat() ? Enums.LevelState.Exploration : Enums.LevelState.Combat);
        
        PlayPattern();
    }

    IEnumerator WaitUntilInteractionAreEnded()
    {
        yield return new WaitUntil(() => interactionPool.activeInteractions.Count <= 0);
        EndLevel();
    }
    
    private void EndLevel()
    {
        GameManager.gameState.SwitchTimeState(Enums.TimeState.Pause);
        UIManager.instance.endLevel.DrawPanel();
        
        patternManager.EndPattern();
    }
    
    public void Restart()
    {
        interactionPool.DisableAllInteractions();
        
        currentIndex = 0;
        
        StreakManager.ResetStreak();
        ScoreManager.ResetScore();
        
        GameManager.gameState.SwitchTimeState(Enums.TimeState.Play);

        InitLevel();
    }
}
