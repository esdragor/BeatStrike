using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pattern : ScriptableObject
{
    public string capacityName;
    public string description;
    public int rarityRank;
    public List<InteractionKey> interractions;

    public void AddPatternKey(InteractionKey pc)
    {
        interractions.Add(pc);
    }

    public void RemovePatternKey(InteractionKey pc)
    {
        interractions.Remove(pc);
    }
    
    void ReorderPatternsList()
    {
        interractions.OrderBy(patternKey => patternKey.timeCode);
    }
}
