using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager
{
    private Dictionary<LevelData, GameObject> levels;
    

    public GameObject GetLevelFromID(int id)
    {
        foreach (var level in levels)
        {
            if (level.Key.id == id)
            {
                return level.Value;
            }
        }
        
        return null;
    }
}
