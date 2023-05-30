using TMPro;
using UnityEngine;
using Utilities;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Interfaces")]
    public UI_HUD hud;

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

    [Header("Objets")] 
    public TMP_Text debugBanditBPM;
    public TMP_Text AnnouncementPatternText;

    public TMP_Text patternDebugTxt;
    public GameObject pausePanel;
    
    private void Awake()
    {
        instance = this;
        tutorial.DrawNext();
    }

    private void Start()
    {
        GameManager.gameState.OnEngineStateChanged += OnEngineStateSetUI;
        hud.textPalierInGame.text = "Palier " + PalierManager.GetPalierText();
    }

    public void DebugPattern(string patternName)
    {
        patternDebugTxt.text = patternName;
    }

    public void OpenLink(string url)
    {
        Debug.Log(url);
        Application.OpenURL(url);
    }

    public void Pause(bool isPause)
    {
        pausePanel.SetActive(isPause);
        
        if (isPause)
        {
            GameManager.gameState.SwitchTimeState(Enums.TimeState.Pause);
        }
        else
        {
            GameManager.gameState.SwitchTimeState(Enums.TimeState.Play);
        }
    }

    public void GoToMainMenu()
    {
        pausePanel.SetActive(false);
        mainMenu.PrintMainMenuPanel();
        hud.DisableHUD();
        GameLoopManager.patternManager.StopPattern();
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
