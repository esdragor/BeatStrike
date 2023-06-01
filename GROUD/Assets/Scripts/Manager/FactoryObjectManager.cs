using UnityEngine;

public class FactoryObjectManager : MonoBehaviour
{
    private static FactoryObjectManager instance;
    
    [SerializeField] private FactoryObject factoryObjectReferenceCommon; 
    [SerializeField] private FactoryObject factoryObjectReferenceEpic; 
    
    private void Awake()
    {
        if (instance == null) instance = this;
    }
    
    public static Gear CreateGear(int rarity)
    {
        FactoryObject factoryObject = 
            rarity == 0 ? instance.factoryObjectReferenceCommon : instance.factoryObjectReferenceEpic;
        Gear gear = ScriptableObject.CreateInstance<Gear>();
        
        int min = 0;
        int max = 0;
        int actualPalier = PalierManager.GetActualPalier();
        
        gear.slot = (GearSlot)Random.Range(0, 3);
        switch (gear.slot)
        {
            case GearSlot.Head:
                gear.statsType1 = StatsType.Hp;
                min = (int)(factoryObject.minHealth + factoryObject.multiplierHealthPerPalier * (float)actualPalier);
                if (min > factoryObject.maxHealth) min = factoryObject.maxHealth;
                max = (int)(min + factoryObject.multiplierHealth * (float)actualPalier);
                if (max > factoryObject.maxHealth) max = factoryObject.maxHealth;
                gear.statsValue1 = Random.Range(min, max);
                break;
            case GearSlot.Chest:
                gear.statsType1 = StatsType.Intelligence;
                min = (int)(factoryObject.minIntelligence + factoryObject.multiplierIntelligencePerPalier * (float)actualPalier);
                if (min > factoryObject.maxIntelligence) min = factoryObject.maxIntelligence;
                max = (int)(min + factoryObject.multiplierIntelligence * (float)actualPalier);
                if (max > factoryObject.maxIntelligence) max = factoryObject.maxIntelligence;
                gear.statsValue1 = Random.Range(min, max);
                break;
            case GearSlot.Weapon:
                gear.statsType1 = StatsType.Strength;
                min = (int)(factoryObject.minStrength + factoryObject.multiplierStrengthPerPalier * (float)actualPalier);
                if (min > factoryObject.maxStrength) min = factoryObject.maxStrength;
                max = (int)(min + factoryObject.multiplierStrength * (float)actualPalier);
                if (max > factoryObject.maxStrength) max = factoryObject.maxStrength;
                gear.statsValue1 = Random.Range(min, max);
                break;
        }
        
        
        gear.rarity = (rarity == 0) ? Rarity.Common : Rarity.Epic;
        
        gear.gearName = gear.slot.ToString();
        gear.gearDescription = $"{gear.rarity} | {gear.statsType1} : {gear.statsValue1}";
        
        gear.priceToBuy = 230 * (rarity + 1) + actualPalier * 10;
        gear.priceToSell = gear.priceToBuy / 2;
        gear.ID = Inventory.GetNewIndexID();
        gear.gearSprite = Inventory.GetSprite(gear.slot);

        return gear;
    }
}
