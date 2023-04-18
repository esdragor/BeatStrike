using System;
using System.Linq;
using Code.Player;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Utilities;

public enum TransitionDirection
{
    Left = -1,
    Right = 1,
    Down = 2,
    Up = 5
}

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;
    
    [HideInInspector] public GearDescription currentGear;
    public PowerDescription currentPower;

    [SerializeField] private RectTransform canvas;

    [Header("Main Menu")] [SerializeField] private GameObject gameTitle;
    [SerializeField] private RectTransform mainMenuButtonsPanel;
    [SerializeField] private float mainMenuFadeInDuration = 1f;
    [SerializeField] private float mainMenuFadeOutDuration = 1f;
    [SerializeField] private TransitionDirection transitionDirectionMainMenu = TransitionDirection.Right;

    [SerializeField] private GameObject GearPrefab;
    [SerializeField] private GameObject powerPrefab;
    [SerializeField] private Image[] equipmentImages;
    [SerializeField] private Transform slotsVoidParent;
    [SerializeField] private Transform slotsEquipementParent;
    [SerializeField] private Transform slotsEquipementPowerParent;

    [SerializeField] private Transform gearSelectionParent;
    [SerializeField] private RectTransform selectionCharacterPanel;
    [SerializeField] private float selectionCharacterFadeInDuration = 1f;
    [SerializeField] private float selectionCharacterFadeOutDuration = 1f;
    [SerializeField] private TransitionDirection transitionDirectionSelectionCharacter = TransitionDirection.Left;
    [SerializeField] private TMP_Text playerInfoText;
    [SerializeField] private Button ButtonEquip;

    [FormerlySerializedAs("GearsDatas")]
    [Header("Game Manager")]
    [SerializeField] private Gear[] gearsDatas;
    [SerializeField] private PowerSO[] powerDatas;

    private Vector2 offsetMainMenu;
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

        offsetMainMenu = mainMenuButtonsPanel.anchoredPosition;

        foreach (var data in powerDatas)
        {
            var newGear = Instantiate(powerPrefab, gearSelectionParent);
            newGear.GetComponent<Image>().sprite = data.powerSprite;
            switch (data.typeOfPower)
            {
                case TypeOfPower.JustPerfect:
                    newGear.GetComponent<PowerDescription>().Power = new JustPerfect();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            {
                
            }
        }

        foreach (var data in gearsDatas)
        {
            var newGear = Instantiate(GearPrefab, gearSelectionParent);
            newGear.GetComponent<Image>().sprite = data.gearSprite;
            newGear.GetComponent<GearDescription>().gear = data;
        }
        ButtonEquip.onClick.AddListener(Equip);
    }

    public void Equip()
    {
        if (currentGear)
        {
            currentGear.gear.EquipOnPlayer(currentGear);
            currentGear.OnEquip = true;
            currentGear = null;
        }
        else if (currentPower)
        {
            currentPower.Power.EquipOnPlayer(currentPower);
            currentPower.OnEquip = true;
            currentPower = null;
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
        gameTitle.SetActive(false);

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

        gameTitle.SetActive(true);
    }

    public void PrintSelectionCharacterPanel()
    {
        if (transitionDirectionSelectionCharacter is TransitionDirection.Down or TransitionDirection.Up)
        {
            selectionCharacterPanel.transform.DOMoveY(Screen.height / 2f, selectionCharacterFadeInDuration);
        }
        else
        {
            selectionCharacterPanel.transform.DOMoveX(Screen.width / 2f, selectionCharacterFadeInDuration);
        }
    }

    public void HideSelectionCharacterPanel()
    {
        if (transitionDirectionSelectionCharacter is TransitionDirection.Down or TransitionDirection.Up)
        {
            selectionCharacterPanel.transform.DOMoveY(((int)transitionDirectionSelectionCharacter - 3) * decal,
                selectionCharacterFadeOutDuration);
        }
        else
        {
            selectionCharacterPanel.transform.DOMoveX((int)transitionDirectionSelectionCharacter * decal,
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

        string hp = AddColor("HP: " + currentCharacterInfos.playerStats.hp, changerStats.hp);

        string intelligence = AddColor("Intelligence: " + currentCharacterInfos.playerStats.intelligence,
            changerStats.intelligence);

        string strength = AddColor("Stamina: " + currentCharacterInfos.playerStats.stamina, changerStats.stamina);

        playerInfoText.text = hp +
                              intelligence +
                              strength;
    }

    public void LaunchGame()
    {
        if (currentCharacterInfos != null)
        {
            SceneManager.LoadScene(1);
            GameManager.instance.gameState.SwitchEngineState(Enums.EngineState.Game);
        }
        else
        {
            Debug.LogError("No character selected");
        }
    }

    public void ExitApplication()
    {
        Application.Quit();
    }

    public void SetPowerImage(PowerDescription powerDescription)
    {
       
        Transform slot = slotsEquipementPowerParent.GetChild(0);
        slot.transform.parent = slotsVoidParent;
        slot.SetSiblingIndex(0);
        powerDescription.transform.parent = slotsEquipementPowerParent;
        powerDescription.transform.SetSiblingIndex(0);
    }
    
    public void SetUnPowerImage(PowerDescription powerDescription)
    {
        int index = 3;
        Transform slot = slotsVoidParent.GetChild(0);
        slot.transform.parent = slotsEquipementPowerParent;
        slot.SetSiblingIndex(index);
        powerDescription.transform.parent = gearSelectionParent;
    }
}