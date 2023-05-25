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

    public MainMenuManager mainMenu;

    [Header("Objets")] 
    public TMP_Text debugBanditBPM;
    public TMP_Text AnnouncementPatternText;
    
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        GameManager.gameState.OnEngineStateChanged += OnEngineStateSetUI;
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
