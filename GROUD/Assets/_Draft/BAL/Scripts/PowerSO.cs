using System;
using Code.Player;
using UnityEngine;

[CreateAssetMenu(fileName = "Power", menuName = "Power", order = 0)]
public class PowerSO : ScriptableObject
{
    public TypeOfPower typeOfPower;
    public Sprite powerSprite;
}


public enum TypeOfPower
{
    JustPerfect,
}