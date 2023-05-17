using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Utilities;

public class GameLoopManager : MonoBehaviour
{
    public static GameLoopManager instance;
    
    public static PatternManager patternManager;
    public static InteractionPool interactionPool;
    public static CombatManager combatManager;
    public static ExplorationManager explorationManager;

    public GameObject[] chunks;
    
    public LevelData levelData;
    public LevelHeader currentChunk;
    
    [Header("Interaction")]
    public Transform midSpawnPoint;
    public Transform interactionParent;
    public InteractionDetector detector;
    public GameObject interactionPrefab;
    
    [Header("EndLevel")]
    public int tickCount;
    [SerializeField] private float speedRun = 10f;
    [SerializeField] private float nbMetersToRun = 10f;
    
    private GameObject currentChunck;
    private GameObject nextChunck;

    private void Awake()
    {
        if (instance == null) instance = this;

        interactionPool = new InteractionPool(interactionParent, interactionPrefab);
        patternManager = new PatternManager();
        combatManager = new CombatManager();
        explorationManager = new ExplorationManager();

        GameObject rndChunk = chunks[Random.Range(0, chunks.Length-1)];
        currentChunck = Instantiate(rndChunk);
        
        GameManager.OnTick += (() => tickCount++);
       // GameManager.OnTick += () => bpmVisual.Play();
    }
    
    public void InitLevel()
    {
        combatManager.PreloadCombat(levelData.enemy);
        PowerManager.AssignNewPower();
        GameManager.gameState.SwitchEngineState(Enums.EngineState.Game);
        GameManager.gameState.SwitchTimeState(Enums.TimeState.Play);
        PlayerManager.instance.SetPlayer();
        PlayPattern();
    }

    private void PlayPattern()
    {
        tickCount = 0;
        
        combatManager.InitCombat();
    }

    public void EndLevel()
    {
        GameManager.gameState.SwitchTimeState(Enums.TimeState.Pause);
        GameManager.gameState.SwitchEngineState(Enums.EngineState.Menu);
        UIManager.instance.endLevel.DrawPanel();
        
        patternManager.EndPattern();
        
    }

    public IEnumerator MoveChunck()
    {
        Transform chunckTr = currentChunck.transform;
        nextChunck.transform.position = chunckTr.position + Vector3.forward * nbMetersToRun;
        
        Vector3 chunchPos = chunckTr.position - Vector3.forward * nbMetersToRun;
        
        while ((chunckTr) && chunckTr.position.z > chunchPos.z)
        {
            currentChunck.transform.position -= Vector3.forward * Time.deltaTime * speedRun;
            nextChunck.transform.position -= Vector3.forward * Time.deltaTime * speedRun;
            yield return new WaitForEndOfFrame();
        }
        
        
        Destroy(currentChunck);
        currentChunck = nextChunck;
        
        if (currentChunk != null)
        {
            levelData = currentChunk.data;
            combatManager.PreloadCombat(levelData.enemy);
            GameManager.gameState.SwitchEngineState(Enums.EngineState.Game);
            GameManager.gameState.SwitchTimeState(Enums.TimeState.Play);
            PlayerManager.instance.SetPlayer();
            PlayPattern();
        }
        else
        {
            EndLevel();
        }
    }

    public void NextChunk()
    {
        GameManager.gameState.SwitchTimeState(Enums.TimeState.Pause);

        patternManager.StopPattern();

        interactionPool.DisableAllInteractions();

        nextChunck = Instantiate(chunks[Random.Range(0, chunks.Length-1)]);
        
        StartCoroutine(MoveChunck());
    }

    public void Restart()
    {
        interactionPool.DisableAllInteractions();

        if (currentChunk != null)
        {
            levelData = currentChunk.data;
            combatManager.PreloadCombat(levelData.enemy);
        }

        StreakManager.ResetStreak();
        ScoreManager.ResetScore();
        
        InitLevel();
    }
}
