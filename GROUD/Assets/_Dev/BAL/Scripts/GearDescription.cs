using UnityEngine;
using UnityEngine.UI;

public class GearDescription : MonoBehaviour
{
    [HideInInspector] public Gear gear;

    private bool OnEquip = false;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
        OnEquip = false;
    }

    public void OnClick()
    {
        if (!OnEquip)
            OnEquip = gear.EquipOnPlayer(this);
        else
             OnEquip = gear.UnequipOnPlayer(this);
    }
}