using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Capacity : ScriptableObject
{
    public string capacityName;
    public string description;
    public int rarityRank;
    public List<PatternKey> patterns;

    public void AddPatternKey(PatternKey pc)
    {
        patterns.Add(pc);
    }

    public void RemovePatternKey(PatternKey pc)
    {
        patterns.Remove(pc);
    }
    
    void ReorderPatternsList()
    {
        patterns.OrderBy(patternKey => patternKey.timeCode);
    }
}
