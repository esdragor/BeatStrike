using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

public class UI_Gear : MonoBehaviour
{
    private static UI_Gear instance;

    [HideInInspector] public GearDescription currentGear;

    [SerializeField] private Button GearButton;
    [SerializeField] private float mainMenuFadeInDuration = 1f;
    [SerializeField] private float mainMenuFadeOutDuration = 1f;
    [SerializeField] private TransitionDirection transitionDirectionMainMenu = TransitionDirection.Right;

    [SerializeField] private GameObject GearPrefab;
    [SerializeField] private Transform slotsEquipementParent;

    [SerializeField] private Transform gearSelectionParent;
    [SerializeField] private RectTransform GearPanel;
    [SerializeField] private float selectionCharacterFadeInDuration = 1f;
    [SerializeField] private float selectionCharacterFadeOutDuration = 1f;
    [SerializeField] private TransitionDirection transitionDirectionSelectionCharacter = TransitionDirection.Left;
    [SerializeField] private TMP_Text playerInfoText;
    [SerializeField] private Button ButtonEquip;
    [SerializeField] private Button ButtonSell;
    [SerializeField] private TMP_Text nbGoldText;

    private CharacterInfos currentCharacterInfos => GameManager.instance.currentCharacterInfos;
    private float decal = 5000f;
    private List<GearDescription> allItems = new List<GearDescription>();

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this)
        {
            Destroy(instance.gameObject);
        }
        CurrencyManager.OnGoldUpdated += UpdateGoldText;
    }



    private void Start()
    {
        PrintCharacterInfos(new PlayerStats());

        ButtonEquip.onClick.AddListener(Equip);
        ButtonSell.onClick.AddListener(Sell);
       // GearButton.onClick.AddListener(HideMainMenuPanel);
        // GearButton.onClick.AddListener(PrintSelectionGearPanel);
        
    }

    public static GearDescription AddItemUIInventory(Gear gear)
    {
        var newGear = Instantiate(instance.GearPrefab, instance.gearSelectionParent);
        newGear.GetComponent<Image>().sprite = gear.gearSprite;
        GearDescription gearDescription = newGear.GetComponent<GearDescription>();
        gearDescription.gear = ScriptableObject.CreateInstance<Gear>();
        gearDescription.gear.CopyGear(gear);
        instance.allItems.Add(gearDescription);
        
        return gearDescription;
    }

    public void UpdateGoldText(int gold)
    {
        nbGoldText.text = gold.ToString();
    }

    public static void ClearInventory()
    {
        while(instance.gearSelectionParent.childCount > 0)
        {
            DestroyImmediate(instance.gearSelectionParent.GetChild(0).gameObject);
        }
    }
    
 

    public static void Equip()
    {
        if (!instance.currentGear) return;
        instance.currentGear.gear.OnEquip = instance.currentGear.gear.EquipOnPlayer(instance.currentGear);
        if (!instance.currentGear.gear.OnEquip) return;
        switch (instance.currentGear.gear.slot)
        {
            case GearSlot.Weapon:
                GearSaveData weaponData = new GearSaveData();
                weaponData.CopyGear(instance.currentGear.gear);
                DataSerializer.instance.SaveDataOnMainDirectory(weaponData, "Weapon");
                break;
            case GearSlot.Chest:
                GearSaveData chestData = new GearSaveData();
                chestData.CopyGear(instance.currentGear.gear);
                DataSerializer.instance.SaveDataOnMainDirectory(chestData, "Chest");
                break;
            case GearSlot.Head:
                GearSaveData headData = new GearSaveData();
                headData.CopyGear(instance.currentGear.gear);
                DataSerializer.instance.SaveDataOnMainDirectory(headData, "Head");
                break;
        }
        instance.currentGear.GetComponent<RectTransform>().localScale = Vector3.one;
        Inventory.RemoveFromInventory(instance.currentGear.gear);
        instance.currentGear = null;
    }
    
    private void Sell()
    {
        if (!instance.currentGear) return;
        CurrencyManager.AddGold(instance.currentGear.gear.priceToSell);
        Inventory.RemoveFromInventory(instance.currentGear.gear);
        PrintCharacterInfos(new PlayerStats());
        RemoveItemUIInventory(instance.currentGear.gear);
    }

    public void SetEquipmentImage(int index, GearDescription gearDescription)
    {
        /*Transform slot = slotsEquipementParent.GetChild(index);
        
        slot.transform.parent = slotsVoidParent;
        slot.SetSiblingIndex(index);
        gearDescription.transform.parent = slotsEquipementParent;
        gearDescription.transform.SetSiblingIndex(index);*/
        
        Transform slot = slotsEquipementParent.GetChild(index);
        gearDescription.transform.parent = slot;
        RectTransform tr = gearDescription.GetComponent<RectTransform>();
        tr.anchorMin = new Vector2(0.5f, 0.5f);
        tr.anchorMax = new Vector2(0.5f, 0.5f);
        tr.localPosition = Vector3.zero;
        tr.SetWidth(100);
        tr.SetHeight(100);
    }

    public void SetUnEquipmentImage(int index, GearDescription gearDescription)
    {

        /*Transform slot = slotsVoidParent.GetChild(0);
        slot.transform.parent = slotsEquipementParent;
        slot.SetSiblingIndex(index);
        gearDescription.transform.parent = gearSelectionParent;*/
        // Transform slot = slotsEquipementParent.GetChild(index);
        
        gearDescription.transform.parent = gearSelectionParent;
    }

    public void PrintSelectionGearPanel()
    {
        if (transitionDirectionSelectionCharacter is TransitionDirection.Down or TransitionDirection.Up)
        {
            GearPanel.transform.DOMoveY(Screen.height / 2f, selectionCharacterFadeInDuration);
        }
        else
        {
            GearPanel.transform.DOMoveX(Screen.width / 2f, selectionCharacterFadeInDuration);
        }
    }

    public void HideSelectionGearPanel()
    {
        if (transitionDirectionSelectionCharacter is TransitionDirection.Down or TransitionDirection.Up)
        {
            GearPanel.transform.DOMoveY(((int)transitionDirectionSelectionCharacter - 3) * decal,
                selectionCharacterFadeOutDuration);
        }
        else
        {
            GearPanel.transform.DOMoveX((int)transitionDirectionSelectionCharacter * decal,
                selectionCharacterFadeOutDuration);
        }
    }

    private string AddColor(string text, float value)
    {
        if (value > 0)
            text += " <color=#00FF00>+" + value + "</color>";
        else if (value < 0)
            text += " <color=#FF0000>" + value + "</color>";
        text += "\n";
        return text;
    }

    public void PrintCharacterInfos(PlayerStats changerStats)
    {
        string hp = AddColor("HP: " + currentCharacterInfos.playerStats.hp, changerStats.hp);

        string intelligence = AddColor("Intelligence: " + currentCharacterInfos.playerStats.intelligence,
            changerStats.intelligence);

        string strength = AddColor("Strength: " + currentCharacterInfos.playerStats.strength, changerStats.strength);

        playerInfoText.text = hp +
                              intelligence +
                              strength;
    }

    public static void RemoveItemUIInventory(Gear gear)
    {
        foreach (var gearDescription in instance.allItems)
        {
            if (gearDescription.gear.ID == gear.ID)
            {
                instance.allItems.Remove(gearDescription);
                Destroy(gearDescription.gameObject);
                instance.currentGear = null;
                return;
            }
        }
    }
}