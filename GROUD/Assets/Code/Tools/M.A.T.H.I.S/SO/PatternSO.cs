using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(order = 0, menuName = "Pattern/Create Pattern", fileName = "new Pattern")]
public class PatternSO : ScriptableObject
{
    public double maxTime = 10f;
    public List<InteractionKey> interactions;
    
    [Button("Reorder List")]
    public void ReorderList()
    {
        if(interactions == null) return;
        interactions = interactions.OrderBy(it => it.timeCode).ToList();
    }
}