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
            MainMenuManager.instance.currentGear = this;
            //gear.EquipOnPlayer(this);
        }
        else
             OnEquip = gear.UnequipOnPlayer(this);
    }
}