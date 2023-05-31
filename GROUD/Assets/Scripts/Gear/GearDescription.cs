using UnityEngine;
using UnityEngine.UI;

public class GearDescription : MonoBehaviour
{
    public bool OnEquip = false;
    public bool clickable = true;
    public bool OnSell = false;
    [HideInInspector] public Gear gear;


    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
        OnEquip = false;
    }

    public void OnClick()
    {
        if (!clickable) return;
        if (OnSell)
        {
            UI_Shop.ShowPopUpBuyItem(this);
            return;
        }

        if (!OnEquip)
        {
            UIManager.instance.gear.currentGear = this;
            PlayerStats stats = new PlayerStats();
            if (gear.statsType1 == StatsType.Hp) stats.hp = gear.statsValue1;
            if (gear.statsType1 == StatsType.Intelligence) stats.intelligence = gear.statsValue1;
            if (gear.statsType1 == StatsType.Strength) stats.strength = gear.statsValue1;

            UIManager.instance.gear.PrintCharacterInfos(stats);
        }
        else
            OnEquip = gear.UnequipOnPlayer(this);
    }
}