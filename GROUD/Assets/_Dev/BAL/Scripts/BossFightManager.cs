using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossFightManager : MonoBehaviour
{
    public static BossFightManager Instance;
    public GameObject PanelInteractions;
    [HideInInspector] public bool isBossTurn = false;

    [SerializeField] private BossPattern bossPattern;
    [SerializeField] private PlayerPattern[] Players;
    [SerializeField] private Button[] capacityButtons;
    [SerializeField] private Slider BossLifeBar;
    [SerializeField] private Slider BossLifeBarSmooth;
    [SerializeField] private TMP_Text LifeText;

    private byte TurnOrderIndex = 0;
    private int life = 3;

    private void Awake()
    {
        Instance = this;
    }

    private void UseCapacity(int index)
    {
        if (!(TurnOrderIndex == index && !isBossTurn)) return;
        PanelInteractions.SetActive(true);
        NextTurn();


        Players[index].LaunchPattern();
    }

    private void NextTurn()
    {
        Players[TurnOrderIndex].GetComponent<Image>().color = Color.white;
        TurnOrderIndex++;
        isBossTurn = true;
        if (TurnOrderIndex >= Players.Length)
            TurnOrderIndex = 0;
    }

    public void FailedToMiss()
    {
        life--;
        LifeText.text = $"Life Remaining : {life}";
    }

    public void EndBossTurn()
    {
        Players[TurnOrderIndex].GetComponent<Image>().color = Color.green;
        isBossTurn = false;
        PanelInteractions.SetActive(false);
    }

    public void BossTakeDamage(float damage)
    {
        bossPattern.CurrentHealth -= damage;
        BossLifeBarSmooth.DOValue(bossPattern.CurrentHealth / bossPattern.MaxHealth, 0.75f).Play();

        if (bossPattern.CurrentHealth < 0f)
            bossPattern.CurrentHealth = 0f;
        BossLifeBar.value = bossPattern.CurrentHealth / bossPattern.MaxHealth;
        if (bossPattern.CurrentHealth <= 0f)
        {
            Destroy(gameObject);
        }

        bossPattern.LaunchPattern(TurnOrderIndex);
    }

    private void OnEnable()
    {
        capacityButtons[0].onClick.RemoveAllListeners();
        capacityButtons[1].onClick.RemoveAllListeners();
        capacityButtons[2].onClick.RemoveAllListeners();

        capacityButtons[0].onClick.AddListener(() => { UseCapacity(0); });
        capacityButtons[1].onClick.AddListener(() => { UseCapacity(1); });
        capacityButtons[2].onClick.AddListener(() => { UseCapacity(2); });

        PanelInteractions.SetActive(false);
        BossLifeBar.value = 1f;
        BossLifeBarSmooth.value = 1f;
        TurnOrderIndex = 0;
        life = 3;
        Players[TurnOrderIndex].GetComponent<Image>().color = Color.green;
    }
}