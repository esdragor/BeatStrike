using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities;

[Serializable] public class InteractionKey : KeyClass
{
    public int row;
    public string timeCode;
    public float tolerance;
    public float duration => outputTime - time;
    public float outputTime;
    public Enums.InteractionType interactionType;
    public List<ConnectorKey> connectors;
    
    public InteractionKey(int row, float time, string timeCode, Enums.InteractionType interactionType)
    {
        this.row = row;
        this.interactionType = interactionType;
        this.timeCode = timeCode;
        this.time = time;
    }

    public void SortConnectors()
    {
        connectors = connectors.OrderBy(c => c.time).ToList();
    }
}

public class ConnectorKey
{
    public int row;
    public float time;

    public ConnectorKey(int row, float time)
    {
        this.row = row;
        this.time = time;
    }
 
}
