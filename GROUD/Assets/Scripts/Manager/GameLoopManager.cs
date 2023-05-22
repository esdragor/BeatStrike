using System;
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

    public GameObject[] chunks;

    public LevelData levelData;
    public LevelHeader currentChunk;

    [Header("Interaction")] public Transform midSpawnPoint;
    public Transform interactionParent;
    public InteractionDetector detector;
    public GameObject interactionPrefab;

    [Header("EndLevel")] public float tickCount;
    [SerializeField] private float speedRun = 10f;
    [SerializeField] private float nbMetersToRun = 10f;
    [SerializeField] private InputManager inputManager;

    [Header("Pattern")] [SerializeField] private List<Pattern> ATKPatterns;
    [SerializeField] private List<Pattern> DEFPatterns;
    [SerializeField] private int percentageDEF;
    [SerializeField] private Material isDefPrinter;


    private GameObject currentChunck;
    private GameObject nextChunck;
    private bool isMoving = false;
    private int index = -1;

    private void Awake()
    {
        if (instance == null) instance = this;

        interactionPool = new InteractionPool(interactionParent, interactionPrefab);
        patternManager = new PatternManager(ATKPatterns, DEFPatterns, percentageDEF);
        combatManager = new CombatManager();

        GameObject rndChunk = chunks[0];
        currentChunck = Instantiate(rndChunk);

        isMoving = false;
    }

    public void AddTickCount(float value)
    {
        tickCount += value;
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
        Debug.Log("PlayPattern");

        combatManager.InitCombat();
    }

    public void EndLevel()
    {
        GameManager.gameState.SwitchTimeState(Enums.TimeState.Pause);
        GameManager.gameState.SwitchEngineState(Enums.EngineState.Menu);
        UIManager.instance.endLevel.DrawPanel();
        patternManager.StopPattern();
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

        PlayerManager.instance.animator.SetBool("isRunning", false);
        Destroy(currentChunck);
        currentChunck = nextChunck;

        if (currentChunk != null)
        {
            levelData = currentChunk.data;
            combatManager.PreloadCombat(levelData.enemy);
            GameManager.gameState.SwitchEngineState(Enums.EngineState.Game);
            GameManager.gameState.SwitchTimeState(Enums.TimeState.Play);
            PlayerManager.instance.SetPlayer();
            isMoving = false;
            PlayPattern();
        }
        else
        {
            EndLevel();
        }
    }

    public void NextChunk()
    {
        isMoving = true;
        PlayerManager.instance.animator.SetBool("isRunning", true);
        GameManager.gameState.SwitchTimeState(Enums.TimeState.Pause);

        patternManager.StopPattern();
        index++;
        int indexrdn = (index + chunks.Length < PalierManager.GetIndexPalier())
            ? 0
            : index + chunks.Length - PalierManager.GetIndexPalier();
        if (indexrdn >= chunks.Length)
        {
            indexrdn = 0;
            index = -1;
        }

        nextChunck = Instantiate(chunks[indexrdn]);

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
        inputManager.gameObject.SetActive(true);
    }

    public void printDEFRoad(bool isDef)
    {
        isDefPrinter.SetInt("_isAttacking", isDef ? 0 : 1);
    }
    
    public bool IsMoving => isMoving;

}