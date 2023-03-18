using UnityEngine;

public class LevelRound : ScriptableObject
{
    public Pattern[] patterns;

    public bool IsBossRound()
    {
        return GetType() == typeof(BossLevelRound);
    }
}