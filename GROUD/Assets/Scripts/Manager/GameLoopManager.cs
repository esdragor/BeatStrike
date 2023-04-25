using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Utilities;
using Random = UnityEngine.Random;

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
    
    private int currentIndex;

    private void Awake()
    {
        if (instance == null) instance = this;

        interactionPool = new InteractionPool(interactionParent, interactionPrefab);
        patternManager = new PatternManager();
        combatManager = new CombatManager();
        explorationManager = new ExplorationManager();
    }

    #region DEBUGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGG

    private void Start()
    {
        GameManager.OnTick += MoveTiles;
    }

    private void MoveTiles()
    {
        // int delayBetweenPatternInMilliseconds = 500;
        // await Task.Delay(delayBetweenPatternInMilliseconds);
        List<GameObject> tiles = interactionPool.GetInteractionPool();
        foreach (var tile in tiles)
        {
            tile.transform.position += Vector3.back * 0.8f;
        }

        //MoveTiles();
    }

    #endregion
    



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
