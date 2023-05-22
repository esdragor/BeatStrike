using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UI_Shop : MonoBehaviour
{
    private static UI_Shop instance;

    [Header("Main Menu")] [SerializeField] private RectTransform panel;
    [SerializeField] private RectTransform mainMenuButtonsPanel;
    [SerializeField] private float mainMenuFadeInDuration = 1f;
    [SerializeField] private float mainMenuFadeOutDuration = 1f;
    [SerializeField] private Button exitBtn;
    [SerializeField] private Button openBtn;
    
    [Header("Currency")] [SerializeField] private TMP_Text textGold;
    [SerializeField] private TMP_Text textKey;
    [SerializeField] private TMP_Text timeToReload;
    
    [Header("Gear Gold")] [SerializeField] private Transform ParentGoldGear;
    [SerializeField] private GameObject gearPrefabShop;
    
    [Header("Gear Key")] [SerializeField] private Button commonChest;
    [SerializeField] private Button epicChest;
    [SerializeField] private Transform parentLootGearChest;
    [SerializeField] private int ticketPriceCommon = 1;
    [SerializeField] private int ticketPriceEpic = 5;
    
    [Header("PopUp")] [SerializeField] private UI_PopUp PopUp;
    [SerializeField] private TMP_Text PopUpText1;
    [SerializeField] private TMP_Text PopUpText2;
    [SerializeField] private Image PopUpImage;
    [SerializeField] private Button PopUpButton;

    private float decal = 5000f;
    private float timeToReloadValue = -99f;
    
    private GearDescription currentGearDescription;

    private void Awake()
    {
        if (instance == null) instance = this;
        else
        {
            Destroy(instance.gameObject);
            instance = this;
        }

        CurrencyManager.OnGoldUpdated += UpdateGoldText;
        CurrencyManager.OnKeysUpdated += UpdateKeyText;

        openBtn.onClick.AddListener(() => GameManager.onUpdated += CheckTime);
        exitBtn.onClick.AddListener(() => GameManager.onUpdated -= CheckTime);
        exitBtn.onClick.AddListener(() => GameManager.onUpdated -= CheckTime);

        commonChest.onClick.AddListener(OpenCommonChest);
        epicChest.onClick.AddListener(OpenEpicChest);
        currentGearDescription = null;
    }

    public void PrintChest(int ID)
    {
        Inventory.AddItemOnInventory(ID);

        if (parentLootGearChest.childCount > 0)
            Destroy(parentLootGearChest.GetChild(0).gameObject);

        Transform tr = Instantiate(gearPrefabShop, parentLootGearChest).transform;
        GearDescription gd = tr.GetChild(0).GetComponent<GearDescription>();
        gd.clickable = false;
        Gear gear = Inventory.GetGearsData().First(g => g.ID == ID);
        gd.GetComponent<Image>().sprite = gear.gearSprite;
        gd.gear = ScriptableObject.CreateInstance<Gear>();
        gd.gear.CopyGear(gear);
        Destroy(gd.transform.parent.gameObject, 5f);
        
        tr.GetChild(1).GetComponent<TMP_Text>().text = gear.priceToBuy.ToString();
    }

    private void OpenCommonChest()
    {
        if (CurrencyManager.GetKeys() >= ticketPriceCommon)
        {
            CurrencyManager.RemoveKeys(ticketPriceCommon);

            Gear[] gears = Inventory.GetGearsData().Where(gear => gear.rarity == Rarity.Common).ToArray();

            int ID = gears[Random.Range(0, gears.Length)].ID;
            PrintChest(ID);
        }
    }

    private void OpenEpicChest()
    {
        if (CurrencyManager.GetKeys() >= ticketPriceEpic)
        {
            CurrencyManager.RemoveKeys(ticketPriceEpic);
            Gear[] gears = Inventory.GetGearsData().Where(gear => gear.rarity <= Rarity.Epic).ToArray();

            int ID = gears[Random.Range(0, gears.Length)].ID;

            PrintChest(ID);
        }
    }

    private void UpdateKeyText(int key)
    {
        textKey.text = key.ToString();
    }

    private void UpdateGoldText(int gold)
    {
        textGold.text = gold.ToString();
    }

    public static void ShowPopUpBuyItem(GearDescription gd)
    {
        instance.currentGearDescription = gd;
        instance.PopUp.TogglePopUp(true);
        instance.PopUpText1.text = gd.gear.getStatType(gd.gear.statsType1) + " : " + gd.gear.statsValue1;
        instance.PopUpText2.text = gd.gear.getStatType(gd.gear.statsType2) + " : " + gd.gear.statsValue2;
        instance.PopUpImage.sprite = gd.gear.gearSprite;
        instance.PopUpButton.onClick.RemoveAllListeners();
        instance.PopUpButton.onClick.AddListener(() => instance.BuyItem());
    }

    private void BuyItem()
    {
        if (currentGearDescription != null)
        {
            if (CurrencyManager.GetGold() >= currentGearDescription.gear.priceToBuy)
            {
                CurrencyManager.RemoveGold(currentGearDescription.gear.priceToBuy);
                Inventory.AddItemOnInventory(currentGearDescription.gear.ID);
                Destroy(currentGearDescription.gameObject);
                currentGearDescription = null;
                instance.PopUp.TogglePopUp(false);
            }
        }
    }

    private void PrintGoldGear()
    {
        Gear[] gears = Inventory.GetGearsData();
        for (int i = 0; i < 3; i++)
        {
            Transform tr = Instantiate(gearPrefabShop, ParentGoldGear).transform;
            GearDescription gd = tr.GetChild(0).GetComponent<GearDescription>();
            gd.OnSell = true;
            Gear gear = gears[Random.Range(0, gears.Length)];
            gd.GetComponent<Image>().sprite = gear.gearSprite;
            gd.gear = ScriptableObject.CreateInstance<Gear>();
            gd.gear.CopyGear(gear);
        
            tr.GetChild(1).GetComponent<TMP_Text>().text = gear.priceToBuy.ToString();
        }
    }

    public void UpdateShop()
    {
        while (ParentGoldGear.childCount > 0)
        {
            DestroyImmediate(ParentGoldGear.GetChild(0).gameObject);
        }

        PrintGoldGear();
    }

    public void CheckTime()
    {
        double lastTime = GameManager.instance.GetLastShopReload();
        if (lastTime > timeToReloadValue)
            UpdateShop();
        timeToReloadValue = (float)lastTime;
        TimeSpan ts = TimeSpan.FromSeconds(lastTime);
        timeToReload.text = $"{ts.Minutes:00}:{ts.Seconds:00}";
    }
}