using UnityEngine;

[CreateAssetMenu(order = 0, fileName = "Level Data", menuName = "Level/Level Data")]
public class LevelData : ScriptableObject
{
    public int id;
    
    public EnemySO enemy;
}