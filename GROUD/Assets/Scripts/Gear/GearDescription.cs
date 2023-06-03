using System;
using UnityEngine;
using UnityEngine.UI;

public class GearDescription : MonoBehaviour
{
    public bool clickable = true;
    public bool OnSell = false;
    public Gear gear;


    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        if (!clickable) return;
        if (OnSell)
        {
            UI_Shop.ShowPopUpBuyItem(this);
            return;
        }

        if (!gear.OnEquip)
        {
            UIManager.instance.gear.currentGear = this;
            PlayerStats stats = new PlayerStats();
            if (gear.statsType1 == StatsType.Hp)
                stats.hp = GameManager.instance.CharacterInfosPrefab.playerStats.hp - 
                    GameManager.instance.currentCharacterInfos.playerStats.hp + gear.statsValue1;
            if (gear.statsType1 == StatsType.Intelligence)
                stats.intelligence = GameManager.instance.CharacterInfosPrefab.playerStats.intelligence - 
                    GameManager.instance.currentCharacterInfos.playerStats.intelligence + gear.statsValue1;
            if (gear.statsType1 == StatsType.Strength)
                stats.strength = GameManager.instance.CharacterInfosPrefab.playerStats.strength - 
                    GameManager.instance.currentCharacterInfos.playerStats.strength + gear.statsValue1;

            UIManager.instance.gear.PrintCharacterInfos(stats);
        }
        else
            gear.OnEquip = gear.UnequipOnPlayer(this);
    }
}