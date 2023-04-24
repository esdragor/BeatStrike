using UnityEngine;

[CreateAssetMenu(order = 0, fileName = "Level Data", menuName = "Level/Level Data")]
public class LevelData : ScriptableObject
{
    public Pattern[] patterns;
    public EnemySO enemy;
    public float distanceToReach;
}