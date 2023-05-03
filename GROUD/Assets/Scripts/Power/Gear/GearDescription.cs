using UnityEngine;
using UnityEngine.UI;

public class GearDescription : MonoBehaviour
{
    public bool OnEquip = false;
    [HideInInspector] public Gear gear;


    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
        OnEquip = false;
    }

    public void OnClick()
    {
        if (!OnEquip)
        {
            UIManager.instance.gear.currentGear = this;
            PlayerStats stats = new PlayerStats();
            if (gear.statsType1 == StatsType.Hp) stats.hp = gear.statsValue1;
            if (gear.statsType1 == StatsType.Intelligence) stats.intelligence = gear.statsValue1;
            if (gear.statsType1 == StatsType.Strength) stats.stamina = gear.statsValue1;

            if (gear.statsType2 == StatsType.Hp) stats.hp = gear.statsValue2;
            if (gear.statsType2 == StatsType.Intelligence) stats.intelligence = gear.statsValue2;
            if (gear.statsType2 == StatsType.Strength) stats.stamina = gear.statsValue2;
            
            UIManager.instance.gear.PrintCharacterInfos(stats);
        }
        else
            OnEquip = gear.UnequipOnPlayer(this);
    }
}