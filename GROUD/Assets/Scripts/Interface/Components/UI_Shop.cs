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
    [SerializeField] private Transform parentLootGearChestEpic;
    [SerializeField] private int ticketPriceCommon = 1;
    [SerializeField] private int ticketPriceEpic = 5;
    
    [Header("PopUp")] [SerializeField] private UI_PopUp PopUp;
    [SerializeField] private TMP_Text PopUpText1;
    [SerializeField] private TMP_Text PopUpText2;
    [SerializeField] private Image PopUpImage;
    [SerializeField] private Button PopUpButton;

    [Header("Chest PopUp")] 
    [SerializeField] private UI_PopUp chestPopUp;
    [SerializeField] private TMP_Text nameTxt;
    [SerializeField] private TMP_Text descriptionTxt;
    [SerializeField] private Image chestPopIllustration;

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

    public void PrintChest(Gear _gear, bool isEpic)
    {
        Inventory.AddItemOnInventory(_gear);

        if (((isEpic) ? parentLootGearChestEpic : parentLootGearChest).childCount > 0)
            Destroy(((isEpic) ? parentLootGearChestEpic : parentLootGearChest).GetChild(0).gameObject);

        Transform tr = Instantiate(gearPrefabShop, (isEpic) ? parentLootGearChestEpic : parentLootGearChest).transform;
        GearDescription gd = tr.GetChild(0).GetComponent<GearDescription>();
        gd.clickable = false;
        gd.GetComponent<Image>().sprite = _gear.gearSprite;
        gd.transform.GetChild(0).GetComponent<Image>().sprite = _gear.gearSprite;
        gd.gear = ScriptableObject.CreateInstance<Gear>();
        gd.gear.CopyGear(_gear);
        Destroy(gd.transform.parent.gameObject, 5f);
        tr.GetChild(1).GetComponent<TMP_Text>().text = _gear.priceToBuy.ToString();

        DrawChestPopUp(_gear);
    }

    public void DrawChestPopUp(Gear gear)
    {
        chestPopUp.TogglePopUp(true);
        
        nameTxt.text = gear.gearName;
        descriptionTxt.text = gear.gearDescription;
        chestPopIllustration.sprite = gear.gearSprite;
    }

    private void OpenCommonChest()
    {
        if (CurrencyManager.GetKeys() >= ticketPriceCommon)
        {
            CurrencyManager.RemoveKeys(ticketPriceCommon);
            
            PrintChest(FactoryObjectManager.CreateGear(0), false);
        }
    }

    private void OpenEpicChest()
    {
        if (CurrencyManager.GetKeys() >= ticketPriceEpic)
        {
            CurrencyManager.RemoveKeys(ticketPriceEpic);
            
            PrintChest(FactoryObjectManager.CreateGear(1), true);
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
                Inventory.AddItemOnInventory(currentGearDescription.gear);
                Transform tr = currentGearDescription.transform.parent;
                Destroy(tr.GetChild(1).gameObject);
                Destroy(currentGearDescription.gameObject);
                currentGearDescription = null;
                instance.PopUp.TogglePopUp(false);
            }
        }
    }

    private void PrintGoldGear()
    {
        for (int i = 0; i < 3; i++)
        {
            Transform tr = Instantiate(gearPrefabShop, ParentGoldGear).transform;
            GearDescription gd = tr.GetChild(0).GetComponent<GearDescription>();
            gd.OnSell = true;
            Gear gear = FactoryObjectManager.CreateGear(0);
            gd.transform.GetChild(0).GetComponent<Image>().sprite = gear.gearSprite;
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