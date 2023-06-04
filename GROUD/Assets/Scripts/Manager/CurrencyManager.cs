using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static Action<int> OnGoldUpdated;
    public static Action<int> OnKeysUpdated;
    public static Action<int> OnGemsUpdated;

    private static CurrencyManager instance;
    
    [SerializeField] private int nbGold;
    [SerializeField] private int nbKeys;
    [SerializeField] private int nbGems;
    
    private bool onReset = false;

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
        
        if (PlayerPrefs.HasKey("Gems"))
        {
            nbGems = PlayerPrefs.GetInt("Gems");
        }
        else
        {
            nbGems = 0;
        }
        
        OnGoldUpdated?.Invoke(nbGold);
        OnKeysUpdated?.Invoke(nbKeys);
        OnGemsUpdated?.Invoke(nbGems);
    }
    
    public static void AddGems(int nbGems)
    {
        instance.nbGems += nbGems;
        OnGemsUpdated?.Invoke(instance.nbGems);
    }
    
    public static void RemoveGems(int nbGems)
    {
        instance.nbGems -= nbGems;
        OnGemsUpdated?.Invoke(instance.nbGems);
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
    
    public static int GetGems()
    {
        return instance.nbGems;
    }
    
    public static void OnResetValue()
    {
        instance.onReset = true;
    }

    private void OnDestroy()
    {
        if (onReset) return;
        PlayerPrefs.SetInt("Gold", nbGold);
        PlayerPrefs.SetInt("Keys", nbKeys);
        PlayerPrefs.SetInt("Gems", nbGems);
    }


}