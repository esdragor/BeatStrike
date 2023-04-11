using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Gear", menuName = "Gear", order = 0)]
public class Gear : ScriptableObject
{
    public string gearName;
    public Sprite gearSprite;
    public string gearDescription;
    public GearSlot slot;

    [Header("Value")] public StatsType statsType1;
    public float statsValue1;
    public StatsType statsType2;
    public float statsValue2;

    public bool EquipOnPlayer(GearDescription gearDescription)
    {
        CharacterInfos ch = GameManager.instance.currentCharacterInfos;

        if (ch.equipment[(int)slot] != null) return false;

        ch.playerStats.ModifyValue(statsType1, statsValue1);
        ch.playerStats.ModifyValue(statsType2, statsValue2);

        ch.equipment[(int)slot] = this;

        if (MainMenuManager.instance == null) return false;
        MainMenuManager.instance.SetEquipmentImage((int)slot, gearDescription);
        MainMenuManager.instance.PrintCharacterInfos();
        return true;
    }

    public bool UnequipOnPlayer(GearDescription gearDescription)
    {
        CharacterInfos ch = GameManager.instance.currentCharacterInfos;

        ch.playerStats.ModifyValue(statsType1, -statsValue1);
        ch.playerStats.ModifyValue(statsType2, -statsValue2);
        
        ch.equipment[(int)slot] = null;

        if (MainMenuManager.instance == null) return false;
        MainMenuManager.instance.SetUnEquipmentImage((int)slot, gearDescription);
        MainMenuManager.instance.PrintCharacterInfos();
        return false;
    }
}

public enum GearSlot
{
    Head,
    Chest,
    Weapon,
}