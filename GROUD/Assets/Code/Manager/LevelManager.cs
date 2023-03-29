using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public Transform levelObject;
    public Vector3 levelOriginPosition;    
    
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
        levelOriginPosition = levelObject.transform.position;
    }

    public void StartLevel()
    {
        PatternManager.OnPatternEnd += CheckNextPattern;
        PlayPattern();
    }

    public void Restart()
    {
        levelObject.DOKill();
        levelObject.position = levelOriginPosition;
        PlayerManager.instance.SetPlayer();

        currentPatternIndex = 0;
        currentRoundIndex = 0;
        
        UIManager.instance.endLevel.DisablePanel();
        
        StartLevel();
    }

    public void PlayPattern()
    {
        PatternManager.Instance.StartPattern(levelData.rounds[currentRoundIndex].patterns[currentPatternIndex]);
    }

    public void MoveWorld(float distance, float duration, Animator playerAnimator)
    {
        levelObject.DOKill();
        levelObject.DOMoveZ(levelObject.transform.position.z - distance, duration).OnComplete( (() => playerAnimator.SetBool("IsRunning", false)));
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
        yield return new WaitUntil(() => PatternPoolManager.Instance.ActiveCircles.Count <= 0);
        EndLevel();

    }
    
    void EndLevel()
    {
        UIManager.instance.endLevel.DrawPanel();
        PatternManager.OnPatternEnd -= CheckNextPattern;
    }
}
