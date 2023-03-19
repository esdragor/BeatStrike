using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public Transform leftSpawnPoint;
    public Transform rightSpawnPoint;
    
    public InteractionDetector leftDetector;
    public InteractionDetector rightDetector;
    
    public LevelData levelData;
    public int currentPatternIndex = 0;
    public int currentRoundIndex = 0;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void StartLevel()
    {
        //UIManager.PlayStartLevelUI
        PatternManager.OnPatternEnd += CheckNextPattern;
        PlayPattern();
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
            Debug.Log("Liste finis pour les patterns");
            CheckNextRound();
        }
        else
        {
            Debug.Log("Nouveau pattern qui se lance dans le round");
            PlayPattern();
        }
    }

    void CheckNextRound()
    {
        currentRoundIndex++;
        currentPatternIndex = 0;

        if (currentRoundIndex >= levelData.rounds.Length)
        {
            EndLevel();
            return;
        }
        
        if (levelData.rounds[currentRoundIndex].IsBossRound())
        {
            Debug.Log("Boss Round !");
            BossManager.instance.StartBossFight((BossLevelRound)levelData.rounds[currentRoundIndex]);
        }
        else
        {
            Debug.Log("Exploration Round !");
            PlayPattern();
        }
    }
    
    void EndLevel()
    {
        //UIManager.PlayEndLevelUI
        Debug.Log("Level Ended");
        PatternManager.OnPatternEnd -= CheckNextPattern;
    }
}
