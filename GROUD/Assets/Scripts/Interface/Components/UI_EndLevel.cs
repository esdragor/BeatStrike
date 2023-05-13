using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

public class UI_EndLevel : MonoBehaviour
{
    [SerializeField] private TMP_Text textScore;
    [SerializeField] private TMP_Text textKey;
    [SerializeField] private TMP_Text textGold;
    
    RectTransform tr;

    public void DrawPanel()
    {
        gameObject.SetActive(true);
        gameObject.transform.parent.gameObject.SetActive(true);
        textScore.text = $"Score: \n\n{ScoreManager.GetScore()}";
        // tr = Inventory.DropInventory(Rarity.Common).GetComponent<RectTransform>();
        // tr.SetParent(DroppedItemParent);
        // tr.position = Vector3.zero;
        // tr.localScale = Vector3.one;

        int nbKey = 1;
        int nbGold = (int)(ScoreManager.GetScore() * 0.1f);
        
        CurrencyManager.AddKeys(nbKey);
        CurrencyManager.AddGold(nbGold);
        
        textKey.text = $"Key: \n\n{nbKey}";
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
    }

    private void OnDisable()
    {
        if (tr != null)
        {
            Destroy(tr.gameObject);
        }
    }
}