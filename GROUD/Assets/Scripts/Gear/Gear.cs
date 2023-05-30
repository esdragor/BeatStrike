using System;
using UnityEngine;

[Serializable]
struct GearSaveData
{
    public GearSlot slot;
    public Rarity rarity;
    public StatsType statsType1;
    public float statsValue1;
    public StatsType statsType2;
    public float statsValue2;
    public int priceToBuy;
    public int priceToSell;
}

[Serializable]
[CreateAssetMenu(fileName = "Gear", menuName = "Gear", order = 0)]
public class Gear : ScriptableObject
{
    public string gearName;
    public Sprite gearSprite;
    public string gearDescription;
    public GearSlot slot;
    public int ID = -1;
    public Rarity rarity = Rarity.Common;

    [Header("Value")] public StatsType statsType1;
    public float statsValue1;
    public StatsType statsType2;
    public float statsValue2;
    public int priceToBuy = 200;
    public int priceToSell = 10;

    public bool EquipOnPlayer(GearDescription gearDescription)
    {
        CharacterInfos ch = GameManager.instance.currentCharacterInfos;

        if (ch.equipment[(int)slot] != null) return false;

        ch.playerStats.ModifyValue(statsType1, statsValue1);
        ch.playerStats.ModifyValue(statsType2, statsValue2);

        ch.equipment[(int)slot] = this;

        if (MainMenuManager.instance == null) return false;
        UIManager.instance.gear.SetEquipmentImage((int)slot, gearDescription);
        UIManager.instance.gear.PrintCharacterInfos(new PlayerStats());
        return true;
    }

    public string getStatType(StatsType type)
    {
        switch (type)
        {
            case StatsType.Hp:
                return "HP";
            case StatsType.Intelligence:
                return "Intelligence";
            case StatsType.Strength:
                return "Strength";
            default:
                return "";
        }
    }

    public bool UnequipOnPlayer(GearDescription gearDescription)
    {
        CharacterInfos ch = GameManager.instance.currentCharacterInfos;

        ch.playerStats.ModifyValue(statsType1, -statsValue1);
        ch.playerStats.ModifyValue(statsType2, -statsValue2);

        switch (ch.equipment[(int)slot].slot)
        {
            case GearSlot.Weapon:
                PlayerPrefs.DeleteKey("Weapon");
                break;
            case GearSlot.Chest:
                PlayerPrefs.DeleteKey("Chest");
                break;
            case GearSlot.Head:
                PlayerPrefs.DeleteKey("Head");
                break;
        }
        ch.equipment[(int)slot] = null;

        if (MainMenuManager.instance == null) return false;
        UIManager.instance.gear.SetUnEquipmentImage((int)slot, gearDescription);
        UIManager.instance.gear.PrintCharacterInfos(new PlayerStats());
        
        Inventory.AddToInventory(gearDescription.gear);
        return false;
    }

    public void CopyGear(Gear _gear)
    {
        gearName = _gear.gearName;
        gearSprite = _gear.gearSprite;
        gearDescription = _gear.gearDescription;
        slot = _gear.slot;
        ID = _gear.ID;
        rarity = _gear.rarity;
        statsType1 = _gear.statsType1;
        statsValue1 = _gear.statsValue1;
        statsType2 = _gear.statsType2;
        statsValue2 = _gear.statsValue2;
        priceToBuy = _gear.priceToBuy;
        priceToSell = _gear.priceToSell;
    }
}

[Serializable]
public enum GearSlot
{
    Head,
    Chest,
    Weapon,
}

[Serializable]
public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}