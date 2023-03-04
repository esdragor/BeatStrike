using System;
using UnityEngine;
using Utilities;

[Serializable] public class InteractionKey : KeyClass
{
    public int row;
    public float scale;
    public string timeCode;
    public float tolerance;
    public float drawSpeed;
    public Vector3 spawnPosition;
    public Enums.InteractionType interactionType;
    public GameObject target;

    public InteractionKey(int row, float time, string timeCode, Enums.InteractionType keyType)
    {
        this.row = row;
        interactionType = keyType;
        this.timeCode = timeCode;
        this.time = time;
    }
}
