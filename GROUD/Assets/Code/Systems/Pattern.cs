using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(order = 0, menuName = "Pattern/Create Pattern", fileName = "new Pattern")]
public class Pattern : ScriptableObject
{
    public string patternName;
    [Expandable] public List<InteractionKey> interactions = new List<InteractionKey>();

    private void OnValidate()
    {
        ReorderList();
    }

    [Button("Reorder List")]
    public void ReorderList()
    {
        interactions = interactions.OrderBy(it => it.timeCode).ToList();
    }

    public void AddPatternKey(InteractionKey pc)
    {
        interactions.Add(pc);
    }

    public void RemovePatternKey(InteractionKey pc)
    {
        interactions.Remove(pc);
    }
    
    void ReorderPatternsList()
    {
        interactions.OrderBy(patternKey => patternKey.timeCode);
    }
}
