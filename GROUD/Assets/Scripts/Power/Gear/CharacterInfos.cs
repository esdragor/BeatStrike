using System;
using Code.Player;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterInfos", menuName = "CharacterInfos", order = 0)]
public class CharacterInfos : ScriptableObject
{
    public PlayerStats playerStats;
    public Sprite playerSprite;
    public Gear[] equipment;
    
    public void SetPlayerStats(CharacterInfos _CharacterInfos)
    {
        playerStats = new PlayerStats(_CharacterInfos.playerStats);
        playerSprite = _CharacterInfos.playerSprite;
        equipment = new []{null, null, (Gear)null};
    }

    public void ResetCH()
    {
        playerStats.ResetStats();
        playerSprite = null;
        equipment = new []{null, null, (Gear)null};
    }
}