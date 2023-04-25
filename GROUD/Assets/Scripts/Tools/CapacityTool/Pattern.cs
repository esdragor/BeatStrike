using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(order = 0, menuName = "Pattern/Create Pattern", fileName = "new Pattern")]
public class Pattern : ScriptableObject
{
    public string patternName;
    public int targetLevel;
    public int difficultyIndex;
    public double maxTime = 10f;
    public int BPM = 60;
    public List<InteractionKey> interactions;

    
    [Button("Reorder List")]
    public void ReorderList()
    {
        if(interactions == null) return;
        interactions = interactions.OrderBy(it => it.timeCode).ToList();
    }
}
