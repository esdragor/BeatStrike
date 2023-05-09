using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Gear : MonoBehaviour
{
    private static UI_Gear instance;

    [HideInInspector] public GearDescription currentGear;

    [Header("Main Menu")] [SerializeField] private RectTransform mainMenuButtonsPanel;
    [SerializeField] private Button GearButton;
    [SerializeField] private float mainMenuFadeInDuration = 1f;
    [SerializeField] private float mainMenuFadeOutDuration = 1f;
    [SerializeField] private TransitionDirection transitionDirectionMainMenu = TransitionDirection.Right;

    [SerializeField] private GameObject GearPrefab;
    [SerializeField] private Transform slotsVoidParent;
    [SerializeField] private Transform slotsEquipementParent;

    [SerializeField] private Transform gearSelectionParent;
    [SerializeField] private RectTransform GearPanel;
    [SerializeField] private float selectionCharacterFadeInDuration = 1f;
    [SerializeField] private float selectionCharacterFadeOutDuration = 1f;
    [SerializeField] private TransitionDirection transitionDirectionSelectionCharacter = TransitionDirection.Left;
    [SerializeField] private TMP_Text playerInfoText;
    [SerializeField] private Button ButtonEquip;
    [SerializeField] private Button ButtonSell;

    private CharacterInfos currentCharacterInfos => GameManager.instance.currentCharacterInfos;
    private float decal = 5000f;
    private List<GearDescription> allItems = new List<GearDescription>();

    private void Awake()
    {
        if (instance == null) instance = this;
        else
        {
            Destroy(instance.gameObject);
            instance = this;
        }
    }



    private void Start()
    {
        PrintCharacterInfos(new PlayerStats());

        ButtonEquip.onClick.AddListener(Equip);
        ButtonSell.onClick.AddListener(Sell);
        GearButton.onClick.AddListener(HideMainMenuPanel);
        GearButton.onClick.AddListener(PrintSelectionGearPanel);
    }

    public static GearDescription AddItemUIInventory(Gear gear)
    {
        var newGear = Instantiate(instance.GearPrefab, instance.gearSelectionParent);
        newGear.GetComponent<Image>().sprite = gear.gearSprite;
        GearDescription gearDescription = newGear.GetComponent<GearDescription>();
        gearDescription.gear = gear;
        instance.allItems.Add(gearDescription);
        
        return gearDescription;
    }
    
 

    public static void Equip()
    {
        if (!instance.currentGear) return;
        instance.currentGear.OnEquip = instance.currentGear.gear.EquipOnPlayer(instance.currentGear);

        switch (instance.currentGear.gear.slot)
        {
            case GearSlot.Weapon:
                PlayerPrefs.SetInt("Weapon", instance.currentGear.gear.ID);
                break;
            case GearSlot.Chest:
                PlayerPrefs.SetInt("Chest", instance.currentGear.gear.ID);
                break;
            case GearSlot.Head:
                PlayerPrefs.SetInt("Head", instance.currentGear.gear.ID);
                break;
        }

        instance.currentGear = null;
    }
    
    private void Sell()
    {
        if (!instance.currentGear) return;
        Inventory.RemoveItemOnInventory(instance.currentGear.gear.ID);
    }

    public void SetEquipmentImage(int index, GearDescription gearDescription)
    {
        Transform slot = slotsEquipementParent.GetChild(index);
        slot.transform.parent = slotsVoidParent;
        slot.SetSiblingIndex(index);
        gearDescription.transform.parent = slotsEquipementParent;
        gearDescription.transform.SetSiblingIndex(index);
    }

    public void SetUnEquipmentImage(int index, GearDescription gearDescription)
    {
        Transform slot = slotsVoidParent.GetChild(0);
        slot.transform.parent = slotsEquipementParent;
        slot.SetSiblingIndex(index);
        gearDescription.transform.parent = gearSelectionParent;
    }

    public void HideMainMenuPanel()
    {
        if (transitionDirectionMainMenu is TransitionDirection.Down or TransitionDirection.Up)
        {
            mainMenuButtonsPanel.transform.DOMoveY(((int)transitionDirectionMainMenu - 3) * decal,
                mainMenuFadeOutDuration);
        }
        else
        {
            mainMenuButtonsPanel.transform.DOMoveX((int)transitionDirectionMainMenu * decal, mainMenuFadeOutDuration);
        }
    }

    public void PrintMainMenuPanel()
    {
        if (transitionDirectionMainMenu is TransitionDirection.Down or TransitionDirection.Up)
        {
            mainMenuButtonsPanel.transform.DOMoveY(Screen.height / 2f, mainMenuFadeInDuration);
        }
        else
        {
            mainMenuButtonsPanel.transform.DOMoveX(Screen.width / 2f, mainMenuFadeInDuration);
        }
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

    private string AddColor(string text, float value, bool doubleline = false)
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

        string strength = AddColor("Stamina: " + currentCharacterInfos.playerStats.strength, changerStats.strength);

        playerInfoText.text = hp +
                              intelligence +
                              strength;
    }

    public static void RemoveItemUIInventory(Gear gear)
    {
        foreach (var gearDescription in instance.allItems)
        {
            if (gearDescription.gear == gear)
            {
                instance.allItems.Remove(gearDescription);
                Destroy(gearDescription.gameObject);
                instance.currentGear = null;
                return;
            }
        }
    }

    public static GameObject DropItem(Gear gear)
    {
        var newGear = Instantiate(instance.GearPrefab, instance.gearSelectionParent);
        newGear.GetComponent<Image>().sprite = gear.gearSprite;
       newGear.GetComponent<GearDescription>().clickable = false;
        return newGear;
    }
}