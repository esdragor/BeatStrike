using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

public class UI_EndLevel : MonoBehaviour
{
    [SerializeField] private TMP_Text textScore;
    [SerializeField] private TMP_Text textGold;
    
    RectTransform tr;

    public void DrawPanel()
    {
        gameObject.SetActive(true);
        gameObject.transform.parent.gameObject.SetActive(true);
        textScore.text = $"Score: \n\n{ScoreManager.GetScore()}";

        int nbGold = (int)(ScoreManager.GetScore() * 0.1f);
        
        CurrencyManager.AddGold(nbGold);
        
        textGold.text = $"Gold: \n\n{nbGold}";
    }

    public void DisablePanel()
    {
        gameObject.SetActive(false);
    }

    public void Restart()
    {
        DisablePanel();
        GameLoopManager.instance.Restart();
    }

    public void Quit()
    {
        DisablePanel();
        UIManager.instance.hud.DisableHUD();
        UIManager.instance.mainMenu.PrintMainMenuPanel();
        UIManager.instance.ResetOnMainMenu();

    }

    private void OnDisable()
    {
        if (tr != null)
        {
            Destroy(tr.gameObject);
        }
    }
}