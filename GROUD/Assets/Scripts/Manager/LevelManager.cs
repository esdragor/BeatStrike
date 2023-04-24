using System.Collections;
using UnityEngine;
using Utilities;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public static PatternManager patternManager;
    public static InteractionPool interactionPool;
    public static CombatManager combatManager;
    public static ExplorationManager explorationManager;
    
    public LevelData levelData;

    [Header("Interaction")]
    public Transform leftSpawnPoint;
    public Transform midSpawnPoint;
    public Transform rightSpawnPoint;
    public Transform interactionParent;
    public InteractionDetector detector;
    
    private int currentIndex;

    private void Awake()
    {
        if (instance == null) instance = this;

        interactionPool = new InteractionPool(interactionParent);
        patternManager = new PatternManager();
        combatManager = new CombatManager();
    }

    public void InitLevel()
    {
        GameManager.gameState.SwitchLevelState(Enums.LevelState.Exploration);
        
        PlayPattern();
    }

    private void PlayPattern()
    {
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
