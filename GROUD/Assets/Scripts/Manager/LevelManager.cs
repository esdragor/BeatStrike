using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using Utilities;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [Header("Interaction")] public Transform leftSpawnPoint;
    public Transform midSpawnPoint;
    public Transform rightSpawnPoint;

    public InteractionDetector detector;

    public LevelData levelData;
    public int currentPatternIndex = 0;
    public int currentRoundIndex = 0;
    public Vector3[] spinPoints;
    public float DistanceToSpawnPointSpin = 30;
    
    [SerializeField] private int delayBetweenPatternInMilliseconds = 400;
    
    private float Speed = 1.5f;
    private PatternPoolManager patternPoolManager;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        patternPoolManager = PatternPoolManager.Instance;
        MoveTiles();
    }

    private async void MoveTiles()
    {
        delayBetweenPatternInMilliseconds = 500;
        await Task.Delay(delayBetweenPatternInMilliseconds);
        foreach (var tile in patternPoolManager.ActiveInteractions)
        {
            tile.transform.position += Vector3.back * 1.2f;
        }

        MoveTiles();
    }

    public void StartLevel()
    {
        GameManager.instance.gameState.SwitchLevelState(Enums.LevelState.Exploration);
        //PlayerManager.instance.MovePlayerTo(Vector3.zero, true);
        PatternManager.OnPatternEnd += CheckNextPattern;
        PlayPattern();
    }

    public void Restart()
    {
        PatternPoolManager.Instance.DisableAllInteractions();

        PlayerManager.instance.SetPlayer();

        currentPatternIndex = 0;
        currentRoundIndex = 0;
        StreakManager.ResetStreak();
        ScoreManager.ResetScore();

        GameManager.instance.gameState.SwitchTimeState(Enums.TimeState.Play);

        StartLevel();
    }

    public void PlayPattern()
    {
        PatternManager.Instance.StartPattern(levelData.patterns[currentPatternIndex]);
    }

    public void SetCombatMode()
    {
        EnemyManager.instance.SetEnemy(levelData.enemy);
        GameManager.instance.gameState.SwitchLevelState(Enums.LevelState.Combat);
    }

    void CheckNextPattern()
    {
        currentPatternIndex++;

        if (currentPatternIndex >= levelData.patterns.Length)
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

        if (currentRoundIndex >= levelData.patterns.Length)
        {
            StartCoroutine(WaitUntilInteractionAreEnded());
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