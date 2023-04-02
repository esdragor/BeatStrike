using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Utilities;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    
    [Header("Road")]
    public LevelRoadManager roadManager;
    public Transform playerObject;
    public float scrollSpeed;
    
    [Header("Interaction")]
    public Transform leftSpawnPoint;
    public Transform midSpawnPoint;
    public Transform rightSpawnPoint;
    
    public InteractionDetector detector;
    
    public LevelData levelData;
    public int currentPatternIndex = 0;
    public int currentRoundIndex = 0;
    public Vector3[] spinPoints;
    public float DistanceToSpawnPointSpin = 30;

    [SerializeField] private GameObject TriggerSpinDetectorPrefab;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void StartLevel()
    {
        playerObject.position = roadManager.steps[0].subStepPosition[0];
        PatternManager.OnPatternEnd += CheckNextPattern;
        PlayPattern();
    }
    private void Update()
    {
        if ( PlayerManager.instance.distanceReached >=  levelData.distanceToReach)
        {
            EndLevel();
        }
    }
    public void Restart()
    {
        PatternPoolManager.Instance.DisableAllInteractions();
        
        PlayerManager.instance.SetPlayer();

        currentPatternIndex = 0;
        currentRoundIndex = 0;
        
        GameManager.instance.gameState.SwitchTimeState(Enums.TimeState.Play);

        StartLevel();
    }
    public void PlayPattern()
    {
        PatternManager.Instance.StartPattern(levelData.rounds[currentRoundIndex].patterns[currentPatternIndex]);
    }
    
    void CheckNextPattern()
    {
        currentPatternIndex++;

        if (currentPatternIndex >= levelData.rounds[currentRoundIndex].patterns.Length)
        {
            CheckNextRound();
        }
        else
        {
            PlayPattern();
        }
    }

    void CheckNextRound()
    {
        currentRoundIndex++;
        currentPatternIndex = 0;

        if (currentRoundIndex >= levelData.rounds.Length)
        {
            StartCoroutine(WaitUntilInteractionAreEnded());
            return;
        }
        
        if (levelData.rounds[currentRoundIndex].IsBossRound())
        {
            BossManager.instance.StartBossFight((BossLevelRound)levelData.rounds[currentRoundIndex]);
        }
        else
        {
            PlayPattern();
        }
    }

    IEnumerator WaitUntilInteractionAreEnded()
    {
        yield return new WaitUntil(() => PatternPoolManager.Instance.ActiveInteractions.Count <= 0);
        EndLevel();

    }
    
    public void EndLevel()
    {
        PatternManager.OnPatternEnd -= CheckNextPattern;
        
        GameManager.instance.gameState.SwitchTimeState(Enums.TimeState.Pause);
        UIManager.instance.endLevel.DrawPanel();
        
        PatternManager.Instance.ForceEnd();
    }
}
