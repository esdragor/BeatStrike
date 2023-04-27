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
    public UI_Power power;

    public MainMenuManager mainMenu;

    [Header("Objets")] 
    public GameObject debugPanel;
    
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        GameManager.gameState.OnEngineStateChanged += OnEngineStateSetUI;
        PlayerManager.onComboSuccess += power.SetText;
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
