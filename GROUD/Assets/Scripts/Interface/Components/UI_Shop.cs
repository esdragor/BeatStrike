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
    [SerializeField] private TMP_Text timeToReload;

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
        
        openBtn.onClick.AddListener(() => GameManager.onUpdated += CheckTime);
        exitBtn.onClick.AddListener(() => GameManager.onUpdated -= CheckTime);
    }

    private void UpdateKeyText(int key)
    {
        textKey.text = key.ToString();
    }

    private void UpdateGoldText(int gold)
    {
        textGold.text = gold.ToString();
    }

    public void CheckTime()
    {
        double lastTime = GameManager.instance.GetLastShopReload();
        TimeSpan ts = TimeSpan.FromSeconds(lastTime);
        timeToReload.text = $"{ts.Minutes:00}:{ts.Seconds:00}";
    }

}
