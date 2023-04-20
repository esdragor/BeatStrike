using System.Collections;
using System.Collections.Generic;
using Code.Player;
using UnityEngine;
using UnityEngine.UI;

public class PowerDescription : MonoBehaviour
{
    public bool OnEquip = false;
    [HideInInspector] public Power Power;


    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
        OnEquip = false;
    }

    public void OnClick()
    {
        if (!OnEquip)
        {
            MainMenuManager.instance.currentPower = this;
            MainMenuManager.instance.currentGear = null;
            PlayerStats stats = new PlayerStats();

            MainMenuManager.instance.PrintCharacterInfos(stats);
        }
        else
            OnEquip = Power.UnequipOnPlayer(this);
    }
}
