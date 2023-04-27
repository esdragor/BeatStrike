using UnityEngine;

public class LevelHeader : MonoBehaviour
{ 
    public LevelData data;
    public Transform combatPos;
    public Transform enemySpawnPoint;

    public void InitLevel()
    {
        Instantiate(data.enemy.visual, enemySpawnPoint.position, new Quaternion(0,180,0,0));
    }
}