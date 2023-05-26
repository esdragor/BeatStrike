using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;
using Utilities;

public class GameLoopManager : MonoBehaviour
{
    public static GameLoopManager instance;

    public static PatternManager patternManager;
    public static InteractionPool interactionPool;
    public static CombatManager combatManager;

    public GameObject[] chunks;

    [FormerlySerializedAs("currentChunk")] public LevelHeader currentChunkLevelHeader;

    [Header("Interaction")] public Transform midSpawnPoint;
    public Transform interactionParent;
    public InteractionDetector detector;
    public GameObject interactionPrefab;

    [Header("EndLevel")] public float tickCount;
    [SerializeField] private float speedRun = 10f;
    [SerializeField, ReadOnly] private float sizeOfChuncks = 35f;
    [SerializeField] private InputManager inputManager;

    [Header("Pattern")] [SerializeField] private List<Pattern> ATKPatterns;
    [SerializeField] private List<Pattern> DEFPatterns;
    [SerializeField] private int percentageDEF;
    [SerializeField] private Material isDefPrinter;


    private GameObject currentChunck;
    private GameObject nextChunck;
    private bool isMoving = false;
    private int index = 0;
    private byte patternType = 2;

    private void Awake()
    {
        if (instance == null) instance = this;

        interactionPool = new InteractionPool(interactionParent, interactionPrefab);
        patternManager = new PatternManager(ATKPatterns, DEFPatterns, percentageDEF);
        combatManager = new CombatManager();

        GameObject rndChunk = chunks[0];
        currentChunck = Instantiate(rndChunk);
        currentChunkLevelHeader = currentChunck.GetComponent<LevelHeader>();
        isMoving = false;

    }

    private void Start()
    {
        SpawnNextChunck();
    }

    public void AddTickCount(float value)
    {
        tickCount += value;
    }

    public void InitLevel()
    {
        combatManager.PreloadCombat();
        PowerManager.AssignNewPower();
        GameManager.gameState.SwitchEngineState(Enums.EngineState.Game);
        GameManager.gameState.SwitchTimeState(Enums.TimeState.Play);
        PlayerManager.instance.SetPlayer();
        GameManager.instance.SetRandomBPM();
        PlayPattern();
    }

    private async void PlayPattern()
    {
        while (GameManager.instance.bpmIsRandoming)
        {
            await Task.Delay(100);
        }

        tickCount = 0;

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
        Transform playerTr = PlayerManager.instance.gameObject.transform;
        Transform chunchPos = nextChunck.GetComponent<LevelHeader>().combatPos;

        while ((chunckTr) && playerTr.position.z < chunchPos.position.z)
        {
            currentChunck.transform.position -= Vector3.forward * Time.deltaTime * speedRun;
            nextChunck.transform.position -= Vector3.forward * Time.deltaTime * speedRun;
            yield return new WaitForEndOfFrame();
        }

        PlayerManager.instance.animator.SetBool("isRunning", false);
        Destroy(currentChunck);
        currentChunck = nextChunck;
        currentChunkLevelHeader = currentChunck.GetComponent<LevelHeader>();

        SpawnNextChunck();

        if (currentChunkLevelHeader != null)
        {
            combatManager.PreloadCombat();
            GameManager.gameState.SwitchEngineState(Enums.EngineState.Game);
            GameManager.gameState.SwitchTimeState(Enums.TimeState.Play);
            //PlayerManager.instance.SetPlayer();
            isMoving = false;
            PlayPattern();
        }
        else
        {
            EndLevel();
        }
    }

    public void SpawnNextChunck()
    {
        index++;
        int indexrdn = (index + chunks.Length < PalierManager.GetIndexPalier())
            ? 0
            : index + chunks.Length - PalierManager.GetIndexPalier();
        if (indexrdn >= chunks.Length)
        {
            indexrdn = 0;
            index = -1;
        }

        Debug.Log("Spawn next chunck");
        Debug.Log("chunck length : " + chunks.Length);
        nextChunck = Instantiate(chunks[indexrdn]);
        nextChunck.transform.position = currentChunck.transform.position + Vector3.forward * sizeOfChuncks;
    }

    public void NextChunk()
    {
        isMoving = true;
        PlayerManager.instance.animator.SetBool("isRunning", true);
        GameManager.gameState.SwitchTimeState(Enums.TimeState.Pause);

        patternManager.StopPattern();


        StartCoroutine(MoveChunck());
    }

    public void Restart()
    {
        interactionPool.DisableAllInteractions();

        if (currentChunkLevelHeader != null)
        {
            combatManager.PreloadCombat();
        }

        StreakManager.ResetStreak();
        ScoreManager.ResetScore();

        InitLevel();
        inputManager.gameObject.SetActive(true);
    }

    private IEnumerator printPattern(bool isDef)
    {
        if (patternType != (isDef ? 1 : 0))
        {
            patternType = (byte) (isDef ? 1 : 0);
            UIManager.instance.AnnouncementPatternText.text =(isDef ? "Defense Phase" : "Attack Phase");
            PlayerManager.instance.vfxManager.NotReadyCombo();
        }
        yield return new WaitForSeconds(1f);
        UIManager.instance.AnnouncementPatternText.text = "";
        patternManager.isTimelineActive = true;
        tickCount = 0;
    }
    
    public void printDEFRoad(bool isDef)
    {
        patternManager.isTimelineActive = false;
        UIManager.instance.StartCoroutine(printPattern(isDef));
        isDefPrinter.SetInt("_isAttacking", isDef ? 0 : 1);
        if (isDef)
        {
            if (PlayerManager.instance.GetCurrentPower() != null)
            {
                PlayerManager.instance.SetPower(null);
                ComboPrinter.PrintNewCombo(Array.Empty<ScreenListener.SwipeDirection>());
                ComboPrinter.UpdateMeter(0);
            }
        }
        else
        {
            if (PlayerManager.instance.GetCurrentPower() == null)
                PowerManager.AssignNewPower();
        }
    }

    public bool IsMoving => isMoving;
}