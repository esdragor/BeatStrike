using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    private static CurrencyManager instance;
    
    [SerializeField] private int nbGold;
    [SerializeField] private int nbKeys;

    private void Awake()
    {
        instance = this;
    }
    
    public static void AddGold(int nbGold)
    {
        instance.nbGold += nbGold;
    }
    
    public static void RemoveGold(int nbGold)
    {
        instance.nbGold -= nbGold;
    }
    
    public static int GetGold()
    {
        return instance.nbGold;
    }
    
    public static void AddKeys(int nbKeys)
    {
        instance.nbKeys += nbKeys;
    }
    
    public static void RemoveKeys(int nbKeys)
    {
        instance.nbKeys -= nbKeys;
    }
    
    public static int GetKeys()
    {
        return instance.nbKeys;
    }
}