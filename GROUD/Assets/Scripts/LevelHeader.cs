using UnityEngine;

public class LevelHeader : MonoBehaviour
{ 
    public LevelData data;
    public Transform combatPos;
    public Transform enemySpawnPoint;

    private void Awake()
    {
        OnInitLevel();
    }

    private void OnInitLevel()
    {
        GameLoopManager.instance.currentChunk = this;
        GameLoopManager.instance.levelData = data;
        
        
        InitLevel();
    }
    
    public void InitLevel()
    {
       // Instantiate(data.enemy.visual, enemySpawnPoint.position, new Quaternion(0,180,0,0));
    }
}