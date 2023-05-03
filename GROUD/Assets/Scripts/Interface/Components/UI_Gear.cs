using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Utilities;

public class UI_Gear : MonoBehaviour
{
    private static UI_Gear instance;
    
    [HideInInspector] public GearDescription currentGear;
    
    [Header("Main Menu")]
    [SerializeField] private RectTransform mainMenuButtonsPanel;
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

    [Header("Game Manager")]
    [SerializeField] private Gear[] gearsDatas;

    private CharacterInfos currentCharacterInfos = null;
    private float decal = 5000f;

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
        
        foreach (var data in gearsDatas)
        {
            var newGear = Instantiate(GearPrefab, gearSelectionParent);
            newGear.GetComponent<Image>().sprite = data.gearSprite;
            newGear.GetComponent<GearDescription>().gear = data;
        }
        ButtonEquip.onClick.AddListener(Equip);
        GearButton.onClick.AddListener(HideMainMenuPanel);
        GearButton.onClick.AddListener(PrintSelectionGearPanel);
    }

    public void Equip()
    {
        if (currentGear)
        {
            currentGear.OnEquip = currentGear.gear.EquipOnPlayer(currentGear);
            currentGear = null;
        }
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
        if (!currentCharacterInfos)
            currentCharacterInfos = GameManager.instance.currentCharacterInfos;
        if (!currentCharacterInfos)
        {
            currentCharacterInfos = ScriptableObject.CreateInstance<CharacterInfos>();
            GameManager.instance.currentCharacterInfos = currentCharacterInfos;
        }

        string hp = AddColor("HP: " + currentCharacterInfos.playerStats.hp, changerStats.hp);

        string intelligence = AddColor("Intelligence: " + currentCharacterInfos.playerStats.intelligence,
            changerStats.intelligence);

        string strength = AddColor("Stamina: " + currentCharacterInfos.playerStats.stamina, changerStats.stamina);

        playerInfoText.text = hp +
                              intelligence +
                              strength;
    }
}
