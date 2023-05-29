using UnityEngine;

public class FactoryObjectManager : MonoBehaviour
{
    private static FactoryObjectManager instance;
    
    [SerializeField] private FactoryObject[] factoryObjectReference; // 0 = common, 1 = epic
    
    private void Awake()
    {
        if (instance == null) instance = this;
    }
    
    public static Gear CreateGear(int rarity)
    {
        FactoryObject factoryObject = instance.factoryObjectReference[rarity];
        Gear gear = ScriptableObject.CreateInstance<Gear>();
        
        float min = 0;
        float max = 0;
        int actualPalier = PalierManager.GetActualPalier();
        
        int statType = Random.Range(0, 3);
        switch (statType)
        {
            case 0:
                gear.statsType1 = StatsType.Hp;
                min = factoryObject.minHealth + factoryObject.multiplierHealthPerPalier * (float)actualPalier;
                if (min > factoryObject.maxHealth) min = factoryObject.maxHealth;
                max = min + factoryObject.multiplierHealth * (float)actualPalier;
                if (max > factoryObject.maxHealth) max = factoryObject.maxHealth;
                gear.statsValue1 = Random.Range(factoryObject.minHealth, max);
                break;
            case 1:
                gear.statsType1 = StatsType.Intelligence;
                min = factoryObject.minIntelligence + factoryObject.multiplierIntelligencePerPalier * (float)actualPalier;
                if (min > factoryObject.maxIntelligence) min = factoryObject.maxIntelligence;
                max = min + factoryObject.multiplierIntelligence * (float)actualPalier;
                if (max > factoryObject.maxIntelligence) max = factoryObject.maxIntelligence;
                gear.statsValue1 = Random.Range(factoryObject.minIntelligence, max);
                break;
            case 2:
                gear.statsType1 = StatsType.Strength;
                min = factoryObject.minStrength + factoryObject.multiplierStrengthPerPalier * (float)actualPalier;
                if (min > factoryObject.maxStrength) min = factoryObject.maxStrength;
                max = min + factoryObject.multiplierStrength * (float)actualPalier;
                if (max > factoryObject.maxStrength) max = factoryObject.maxStrength;
                gear.statsValue1 = Random.Range(factoryObject.minStrength, max);
                break;
        }
        
        statType = Random.Range(0, 3);
        switch (statType)
        {
            case 0:
                gear.statsType2 = StatsType.Hp;
                min = factoryObject.minHealth + factoryObject.multiplierHealthPerPalier * (float)actualPalier;
                if (min > factoryObject.maxHealth) min = factoryObject.maxHealth;
                max = factoryObject.minHealth + factoryObject.multiplierHealth * (float)actualPalier;
                if (max > factoryObject.maxHealth) max = factoryObject.maxHealth;
                gear.statsValue2 = Random.Range(factoryObject.minHealth, max);
                break;
            case 1:
                gear.statsType2 = StatsType.Intelligence;
                min = factoryObject.minIntelligence + factoryObject.multiplierIntelligencePerPalier * (float)actualPalier;
                if (min > factoryObject.maxIntelligence) min = factoryObject.maxIntelligence;
                max = factoryObject.minIntelligence + factoryObject.multiplierIntelligence * (float)actualPalier;
                if (max > factoryObject.maxIntelligence) max = factoryObject.maxIntelligence;
                gear.statsValue2 = Random.Range(factoryObject.minIntelligence, max);
                break;
            case 2:
                gear.statsType2 = StatsType.Strength;
                min = factoryObject.minStrength + factoryObject.multiplierStrengthPerPalier * (float)actualPalier;
                if (min > factoryObject.maxStrength) min = factoryObject.maxStrength;
                max = factoryObject.minStrength + factoryObject.multiplierStrength * (float)actualPalier;
                if (max > factoryObject.maxStrength) max = factoryObject.maxStrength;
                gear.statsValue2 = Random.Range(factoryObject.minStrength, max);
                break;
        }
        
        gear.rarity = (rarity == 0) ? Rarity.Common : Rarity.Epic;
        gear.priceToBuy = 230 * (rarity + 1) + actualPalier * 10;
        gear.priceToSell = gear.priceToBuy / 2;
        gear.slot = (GearSlot)Random.Range(0, 3);
        gear.ID = Inventory.GetNewIndexID();

        return gear;
    }
}
