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
    public GearDescription currentGear;

    [SerializeField] private RectTransform canvas;

    [Header("Main Menu")] [SerializeField] private GameObject gameTitle;
    [SerializeField] private RectTransform mainMenuButtonsPanel;
    [SerializeField] private float mainMenuFadeInDuration = 1f;
    [SerializeField] private float mainMenuFadeOutDuration = 1f;
    [SerializeField] private TransitionDirection transitionDirectionMainMenu = TransitionDirection.Right;

    [SerializeField] private GameObject GearPrefab;
    [SerializeField] private Image[] equipmentImages;
    [SerializeField] private Transform slotsVoidParent;
    [SerializeField] private Transform slotsEquipementParent;

    [SerializeField] private Transform gearSelectionParent;
    [SerializeField] private RectTransform selectionCharacterPanel;
    [SerializeField] private float selectionCharacterFadeInDuration = 1f;
    [SerializeField] private float selectionCharacterFadeOutDuration = 1f;
    [SerializeField] private TransitionDirection transitionDirectionSelectionCharacter = TransitionDirection.Left;
    [SerializeField] private TMP_Text playerInfoText;
    [SerializeField] private Button ButtonEquip;

    [Header("Game Manager")]
    [SerializeField] private Gear[] GearsDatas;

    private Vector2 offsetMainMenu;
    private CharacterInfos currentCharacterInfos = null;

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
        PrintCharacterInfos();

        offsetMainMenu = mainMenuButtonsPanel.anchoredPosition;
        foreach (var data in GearsDatas)
        {
            var newGear = Instantiate(GearPrefab, gearSelectionParent);
            newGear.GetComponent<Image>().sprite = data.gearSprite;
            newGear.GetComponent<GearDescription>().gear = data;
        }
        ButtonEquip.onClick.AddListener(Equip);
    }

    public void Equip()
    {
        if (!currentGear) return;
        currentGear.gear.EquipOnPlayer(currentGear);
        currentGear.OnEquip = true;
        currentGear = null;
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
            mainMenuButtonsPanel.transform.DOMoveY(((int)transitionDirectionMainMenu - 3) * 2500f,
                mainMenuFadeOutDuration);
        }
        else
        {
            mainMenuButtonsPanel.transform.DOMoveX((int)transitionDirectionMainMenu * 2500f, mainMenuFadeOutDuration);
        }
    }

    public void PrintMainMenuPanel()
    {
        if (transitionDirectionMainMenu is TransitionDirection.Down or TransitionDirection.Up)
        {
            mainMenuButtonsPanel.transform.DOMoveY(canvas.GetHeight() / 2 + offsetMainMenu.y, mainMenuFadeInDuration);
        }
        else
        {
            mainMenuButtonsPanel.transform.DOMoveX(canvas.GetWidth() / 2 + offsetMainMenu.x, mainMenuFadeInDuration);
        }

        gameTitle.SetActive(true);
    }

    public void PrintSelectionCharacterPanel()
    {
        if (transitionDirectionSelectionCharacter is TransitionDirection.Down or TransitionDirection.Up)
        {
            selectionCharacterPanel.anchoredPosition = new Vector3(0,
                ((int)transitionDirectionSelectionCharacter - 3) * 2500f);
            selectionCharacterPanel.transform.DOMoveY(canvas.GetHeight() / 2, selectionCharacterFadeInDuration);
        }
        else
        {
            selectionCharacterPanel.anchoredPosition =
                new Vector3((int)transitionDirectionSelectionCharacter * 2500f, 0);
            selectionCharacterPanel.transform.DOMoveX(canvas.GetWidth() / 2, selectionCharacterFadeInDuration);
        }
    }

    public void HideSelectionCharacterPanel()
    {
        if (transitionDirectionSelectionCharacter is TransitionDirection.Down or TransitionDirection.Up)
        {
            selectionCharacterPanel.transform.DOMoveY(((int)transitionDirectionSelectionCharacter - 3) * 2500f,
                selectionCharacterFadeOutDuration);
        }
        else
        {
            selectionCharacterPanel.transform.DOMoveX((int)transitionDirectionSelectionCharacter * 2500f,
                selectionCharacterFadeOutDuration);
        }
    }

    public void PrintCharacterInfos()
    {
        if (!currentCharacterInfos)
            currentCharacterInfos = GameManager.instance.currentCharacterInfos;

        playerInfoText.text = "HP: " + currentCharacterInfos.playerStats.hp + "\n" +
                              "Competence Duration: " + currentCharacterInfos.playerStats.competenceDuration + "\n\n" +
                              "Damage: " + currentCharacterInfos.playerStats.damage + "\n" +
                              "Speed: " + currentCharacterInfos.playerStats.speed + "\n\n" +
                              "Crit Rate: " + currentCharacterInfos.playerStats.critRate + "\n" +
                              "Crit Tolerance: " + currentCharacterInfos.playerStats.critTolerance;
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
}