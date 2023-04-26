using UnityEngine;

public class LevelHeader : MonoBehaviour
{ 
    public LevelData data;
    public Transform enemySpawnPoint;

    public void InitLevel()
    {
        Instantiate(data.enemy.visual, enemySpawnPoint.position, Quaternion.identity);
    }
}