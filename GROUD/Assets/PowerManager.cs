using System;
using System.Collections;
using System.Collections.Generic;
using Code.Player;
using UnityEngine;
using Utilities;

public class PowerManager : MonoBehaviour
{
    private static PowerManager instance;

    [SerializeField] private List<PowerSO> powers;

    private void Awake()
    {
        if (instance == null) instance = this;
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