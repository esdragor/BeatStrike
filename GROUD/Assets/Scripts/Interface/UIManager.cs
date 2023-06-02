using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Interfaces")] public UI_HUD hud;

    public UI_Enemy enemy;
    public UI_Announcer announcer;
    public UI_EndLevel endLevel;
    public UI_Score score;
    public UI_Streak streak;
    public UI_Gear gear;
    public UI_Shop shop;
    public UI_RandomBPMSelector randomBPMSelector;
    public UI_Tutorial tutorial;

    public MainMenuManager mainMenu;

    [Header("Objets")] public TMP_Text debugBanditBPM;
    public TMP_Text AnnouncementPatternText;

    public TMP_Text patternDebugTxt;
    public GameObject pausePanel;

    public Image muteIcon;
    public Sprite muteSprite;
    public Sprite unMuteSprite;

    private void Awake()
    {
        instance = this;
        tutorial.DrawNext();
    }

    private void Start()
    {
        GameManager.gameState.OnEngineStateChanged += OnEngineStateSetUI;
    }

    public void UpdatePalier(string palier)
    {
        hud.textPalierInGame.text = "Palier " + palier;
        MainMenuManager.instance.UpdatePalierText(palier);
    }

    private string[] lastPatterns = new string[3];

    public void DebugPattern(string patternName)
    {
        lastPatterns[2] = lastPatterns[1];
        lastPatterns[1] = lastPatterns[0];
        lastPatterns[0] = patternName;

        patternDebugTxt.text = $"{lastPatterns[0]} <br>{lastPatterns[1]} <br>{lastPatterns[2]}";
    }

    public void ResetOnMainMenu()
    {
        PlayerManager.instance.matRune.SetFloat("_AbilityProgress", 0);
        hud.UpdateTimeLine(0);
    }

    public void OpenLink(string url)
    {
        Debug.Log(url);
        Application.OpenURL(url);
    }

    public void Mute()
    {
        muteIcon.sprite = SoundManager.ToggleMute() ? muteSprite : unMuteSprite;
    }

    public void Pause(bool isPause)
    {
        pausePanel.SetActive(isPause);

        if (isPause)
        {
            GameManager.instance.savedTick = GameLoopManager.instance.tickCount;
            GameManager.gameState.SwitchTimeState(Enums.TimeState.Pause);
        }
        else
        {
            GameLoopManager.instance.tickCount = GameManager.instance.savedTick;
            GameManager.gameState.SwitchTimeState(Enums.TimeState.Play);
        }
    }

    public void GoToMainMenu()
    {
        pausePanel.SetActive(false);
        mainMenu.PrintMainMenuPanel();
        hud.DisableHUD();
        GameLoopManager.patternManager.StopPattern();
        ResetOnMainMenu();
    }

    void OnEngineStateSetUI(Enums.EngineState engineState)
    {
        switch (engineState)
        {
            case Enums.EngineState.Menu:
                hud.DisableHUD();
                break;

            case Enums.EngineState.Game:
                hud.EnableHUD();
                break;
        }
    }
}