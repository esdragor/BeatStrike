using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public LevelData levelData;
    private int currentPatternIndex = 0;
    private int currentRoundIndex = 0;

    public void StartLevel()
    {
        //UIManager.PlayStartLevelUI
        PatternManager.OnPatternEnd += CheckNextPattern;
    }

    void PlayPattern()
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
        
        if (currentPatternIndex < levelData.rounds.Length)
        {
            EndLevel();
            return;
        }
        
        if (levelData.rounds[currentRoundIndex].IsBossRound())
        {
            GameManager.instance.StartBoss();
        }
        else
        {
            PlayPattern();
        }
    }
    
    void EndLevel()
    {
        //UIManager.PlayEndLevelUI
        PatternManager.OnPatternEnd -= CheckNextPattern;
    }
}
