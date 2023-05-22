using System;
using UnityEngine;
using Utilities;

[Serializable] public class InteractionKey : KeyClass
{
    [HideInInspector] public int row;
    [HideInInspector] public string timeCode;
    [HideInInspector] public float frame;

    public Enums.InteractionType interactionType;
    public ScreenListener.SwipeDirection swipeDirection;

    public InteractionKey(int row, float time, string timeCode, Enums.InteractionType keyType)
    {
        this.row = row;
        interactionType = keyType;
        this.timeCode = timeCode;
        this.time = time;
    }

    public enum InteractionColor
    {
        Blue,
        Red
    }
}
