using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static Action<int> OnGoldUpdated;
    public static Action<int> OnKeysUpdated;

    private static CurrencyManager instance;
    
    [SerializeField] private int nbGold;
    [SerializeField] private int nbKeys;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("Gold"))
        {
            nbGold = PlayerPrefs.GetInt("Gold");
        }
        else
        {
            nbGold = 100;
        }
        
        if (PlayerPrefs.HasKey("Keys"))
        {
            nbKeys = PlayerPrefs.GetInt("Keys");
        }
        else
        {
            nbKeys = 0;
        }
        
        OnGoldUpdated?.Invoke(nbGold);
        OnKeysUpdated?.Invoke(nbKeys);
    }

    public static void AddGold(int nbGold)
    {
        instance.nbGold += nbGold;
        OnGoldUpdated?.Invoke(instance.nbGold);
    }
    
    public static void RemoveGold(int nbGold)
    {
        instance.nbGold -= nbGold;
        OnGoldUpdated?.Invoke(instance.nbGold);
    }
    
    public static int GetGold()
    {
        return instance.nbGold;
    }
    
    public static void AddKeys(int nbKeys)
    {
        instance.nbKeys += nbKeys;
        OnKeysUpdated?.Invoke(instance.nbKeys);
    }
    
    public static void RemoveKeys(int nbKeys)
    {
        instance.nbKeys -= nbKeys;
        OnKeysUpdated?.Invoke(instance.nbKeys);
    }
    
    public static int GetKeys()
    {
        return instance.nbKeys;
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetInt("Gold", nbGold);
        PlayerPrefs.SetInt("Keys", nbKeys);
    }
}