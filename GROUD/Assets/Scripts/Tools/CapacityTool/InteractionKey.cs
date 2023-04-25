using System;
using UnityEngine;
using Utilities;

[Serializable] public class InteractionKey : KeyClass
{
    public int row;
    public string timeCode;
    public float frame;

    public Enums.InteractionType interactionType;
    public ScreenListener.SwipeDirection swipeDirection;
    public InteractionColor interactionColor;

    public InteractionKey(int row, float time, string timeCode, Enums.InteractionType keyType)
    {
        this.row = row;
        interactionType = keyType;
        this.timeCode = timeCode;
        this.time = time;

        switch (row)
        {
            case 1:
                interactionColor = InteractionColor.Red;
                break;
            
            case 0:
                interactionColor = InteractionColor.Blue;
                break;
        }
    }

    public enum InteractionColor
    {
        Blue,
        Red
    }
}
