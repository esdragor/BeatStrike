using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    public int currentWorld;

    public World world1;
    public World world2;
    public World world3;
    
    public void GenerateNextLevel()
    {
        GameObject rndVisual = null;
        
        LevelData rndLD = null;
        switch (currentWorld)
        {
            case 1: 
                rndVisual = world1.visuals[Random.Range(0, world1.visuals.Length)];
                rndLD = world1.levelDatas[Random.Range(0, world1.levelDatas.Length)];
                break;
            
            case 2:
                rndVisual = world2.visuals[Random.Range(0, world2.visuals.Length)];
                rndLD = world2.levelDatas[Random.Range(0, world2.levelDatas.Length)];
                break;
            
            case 3:
                rndVisual = world3.visuals[Random.Range(0, world3.visuals.Length)];
                rndLD = world3.levelDatas[Random.Range(0, world3.levelDatas.Length)];
                break;
        }

        if (rndVisual != null)
        {
            GameObject level = Instantiate(rndVisual, Vector3.zero, Quaternion.identity);
            LevelHeader header = level.GetComponent<LevelHeader>();
            header.data = rndLD;
            header.InitLevel();
        }

    }
}

[Serializable] public class World
{
    public GameObject[] visuals;
    public LevelData[] levelDatas;
}
