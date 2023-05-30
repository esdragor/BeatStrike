using System;
using Code.Player;
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

    [SerializeField] private TMP_Text textPalier;
    [SerializeField] private Button ResetButton;

    private void Awake()
    {
        if (instance == null) instance = this;
        else
        {
            Destroy(instance.gameObject);
            instance = this;
        }

        ResetButton.onClick.AddListener(ResetPlayerPrefs);
    }

    private void OnEnable()
    {
        UpdatePalierText();
        
    }

    private void Start()
    {
        UpdatePalierText();
    }

    public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        Inventory.OnResetValue();
        CurrencyManager.OnResetValue();
        
        DataSerializer.instance.Reset();

       Application.Quit();
    }

    private void UpdatePalierText()
    {
        if (textPalier && GameManager.instance)
            textPalier.text = "Palier " + PalierManager.GetPalierText();
       
    }

    public void HideMainMenuPanel()
    {
        gameObject.SetActive(false);
        GameManager.gameState.SwitchEngineState(Enums.EngineState.Game);
    }

    public void PrintMainMenuPanel()
    {
        gameObject.SetActive(true);
        GameManager.gameState.SwitchEngineState(Enums.EngineState.Menu);
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

    public void LaunchGame()
    {
        GameLoopManager.instance.InitLevel();
        HideMainMenuPanel();
    }
}