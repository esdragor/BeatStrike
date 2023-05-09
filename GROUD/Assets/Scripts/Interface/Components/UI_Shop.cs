using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UI_Shop : MonoBehaviour
{
    private static UI_Shop instance;
    
    [Header("Main Menu")]
    [SerializeField] private RectTransform panel;
    [SerializeField] private RectTransform mainMenuButtonsPanel;
    [SerializeField] private float mainMenuFadeInDuration = 1f;
    [SerializeField] private float mainMenuFadeOutDuration = 1f;
    [SerializeField] private Button exitBtn;
    [SerializeField] private Button openBtn;
    [Header("Currency")]
    [SerializeField] private TMP_Text textGold;
    [SerializeField] private TMP_Text textKey;

    private float decal = 5000f;

    private void Awake()
    {
        if (instance == null) instance = this;
        else
        {
            Destroy(instance.gameObject);
            instance = this;
        }
        CurrencyManager.OnGoldUpdated += UpdateGoldText;
        CurrencyManager.OnKeysUpdated += UpdateKeyText;
    }

    private void UpdateKeyText(int key)
    {
        textKey.text = key.ToString();
    }

    private void UpdateGoldText(int gold)
    {
        textGold.text = gold.ToString();
    }

    private void Start()
    {
        exitBtn.onClick.AddListener(HideShopPanel);
        exitBtn.onClick.AddListener(PrintMainMenuPanel);
        
        openBtn.onClick.AddListener(HideMainMenuPanel);
        openBtn.onClick.AddListener(PrintShopPanel);
        
    }

    public void HideShopPanel()
    {
        panel.transform.DOMoveX(decal, mainMenuFadeOutDuration);
    }

    public void PrintShopPanel()
    {
        panel.transform.DOMoveX(Screen.width / 2f, mainMenuFadeInDuration);
    }
    
    public void HideMainMenuPanel()
    {
        mainMenuButtonsPanel.transform.DOMoveX((int)-decal, mainMenuFadeOutDuration);
    }

    public void PrintMainMenuPanel()
    {
        mainMenuButtonsPanel.transform.DOMoveX(Screen.width / 2f, mainMenuFadeInDuration);
    }

}
