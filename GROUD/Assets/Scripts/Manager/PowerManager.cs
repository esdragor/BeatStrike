using System.Collections.Generic;
using UnityEngine;

public class PowerManager : MonoBehaviour
{
    private static PowerManager instance;

    [SerializeField] private List<PowerSO> powers;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        PlayerManager.instance.matRune.SetFloat("_AbilityProgress", 0);
    }

    public static void AssignNewPower()
    {
        int rdnPower = UnityEngine.Random.Range(0, instance.powers.Count);

        PowerSO newPower = instance.powers[rdnPower];
        newPower.SetPower();

        newPower.power.OnSetBase();
        PlayerManager.instance.SetPower(newPower);
    }
}