using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    
    [SerializeField] private RectTransform canvas;
    
    [Header("Main Menu")] 
    [SerializeField] private GameObject gameTitle;
    [SerializeField] private RectTransform mainMenuButtonsPanel;
    [SerializeField] private float mainMenuFadeInDuration = 1f;
    [SerializeField] private float mainMenuFadeOutDuration = 1f;
    [SerializeField] private TransitionDirection transitionDirectionMainMenu = TransitionDirection.Right;

    [Header("Selection Character")] 
    [SerializeField] private GameObject PlayerPrefab;
    [SerializeField] private Transform characterSelectionParent;
    [SerializeField] private RectTransform selectionCharacterPanel;
    [SerializeField] private float selectionCharacterFadeInDuration = 1f;
    [SerializeField] private float selectionCharacterFadeOutDuration = 1f;
    [SerializeField] private TransitionDirection transitionDirectionSelectionCharacter = TransitionDirection.Left;
    [SerializeField] private TMP_Text playerInfoText;
    [SerializeField] private Button buttonLockCharacter;
    
    [Header("Game Manager")]
    [SerializeField] private CharacterInfos[] playerDatas;

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
        offsetMainMenu = mainMenuButtonsPanel.anchoredPosition;

        foreach (var data in playerDatas)
        {
            var newChara = Instantiate(PlayerPrefab, characterSelectionParent);
            newChara.GetComponent<PlayerSelectionStats>().SetPlayerStats(data);
        }
        buttonLockCharacter.onClick.AddListener(LockCharacter);
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
            mainMenuButtonsPanel.transform.DOMoveY(canvas.GetHeight()/2 + offsetMainMenu.y, mainMenuFadeInDuration);
        }
        else
        {
            mainMenuButtonsPanel.transform.DOMoveX(canvas.GetWidth()/2 + offsetMainMenu.x, mainMenuFadeInDuration);
        }
        gameTitle.SetActive(true);
    }
    
    public void PrintSelectionCharacterPanel()
    {
        if (transitionDirectionSelectionCharacter is TransitionDirection.Down or TransitionDirection.Up)
        {
            selectionCharacterPanel.anchoredPosition = new Vector3(0,
                ((int)transitionDirectionSelectionCharacter - 3) * 2500f);
            selectionCharacterPanel.transform.DOMoveY(canvas.GetHeight()/2, selectionCharacterFadeInDuration);
        }
        else
        {
            selectionCharacterPanel.anchoredPosition = new Vector3((int)transitionDirectionSelectionCharacter * 2500f,  0);
            selectionCharacterPanel.transform.DOMoveX(canvas.GetWidth()/2, selectionCharacterFadeInDuration);
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
    
    public void PrintCharacterInfos(CharacterInfos characterInfos)
    {
        currentCharacterInfos = new CharacterInfos();
        currentCharacterInfos.playerStats = characterInfos.playerStats;
        currentCharacterInfos.playerSprite = characterInfos.playerSprite;
        currentCharacterInfos.PowerInfo = characterInfos.PowerInfo;
        currentCharacterInfos.power = characterInfos.power;
        
        playerInfoText.text = "HP: " + currentCharacterInfos.playerStats.hp + "\n" +
                              "Speed: " + currentCharacterInfos.playerStats.speed + "\n" +
                              "Tolerance: " + currentCharacterInfos.playerStats.tolerance + "\n" +
                              "Experience Factor: " + currentCharacterInfos.playerStats.experienceFactor + "\n" +
                              "Damage: " + currentCharacterInfos.playerStats.damage + "\n" +
                              "Power: " + currentCharacterInfos.PowerInfo;
    }

    public void LockCharacter()
    {
        if (currentCharacterInfos != null)
        {
            GameManager.instance.SetPlayerStats(currentCharacterInfos);
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