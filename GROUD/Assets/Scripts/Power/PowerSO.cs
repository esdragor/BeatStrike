using System;
using Code.Player;
using UnityEngine;
using Utilities;

[CreateAssetMenu(fileName = "Power", menuName = "Power", order = 0)]
public class PowerSO : ScriptableObject
{
    public  Enums.TypeOfPower typeOfPower;
    public Sprite powerSprite;
    public ComboPower power;
    public float ATKValue;
    
    
    public void LaunchPower()
    {
        
        switch (typeOfPower)
        {
            case Enums.TypeOfPower.Fireball:
                power.OnSuccess(ATKValue);
                break;
        }
    }
    
    public void SetPower()
    {
        
        switch (typeOfPower)
        {
            case Enums.TypeOfPower.Fireball:
                power = new Fireball();
                break;
        }
        
        power.OnSuccessAction = LaunchPower;
    }
}