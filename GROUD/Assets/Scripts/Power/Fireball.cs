using System.Collections;
using System.Collections.Generic;
using Code.Player;
using UnityEngine;

public class Fireball : ComboPower
{
    public override void OnSuccess(float value)
    {
        base.OnSuccess(value);
        
       PlayerManager.instance.HurtEnemy((int)(value * 
                                              GameManager.instance.currentCharacterInfos.playerStats.intelligence));
    }
}
