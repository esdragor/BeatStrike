using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct PlayerData
{
    public int life;
    public int damage;
}

public class BossFightManager : MonoBehaviour
{
    public static BossFightManager Instance;
    [HideInInspector] public bool isBossTurn = false;

    [SerializeField] private PlayerData playerData = new () {life = 3, damage = 30};
    [SerializeField] private BossPattern bossPattern;
    [SerializeField] private Button[] capacityButtons;
    [SerializeField] private Slider BossLifeBar;
    [SerializeField] private Slider BossLifeBarSmooth;
    [SerializeField] private TMP_Text LifeText;

    private byte TurnOrderIndex = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void UseCapacity()
    {
        Debug.Log("Use Capacity");
    }

    public void FailedToMiss()
    {
        playerData.life--;
        LifeText.text = $"Life Remaining : {playerData.life}";
        StartCoroutine(DelayBeforeNextTurn());
    }

    public void BossTakeDamage()
    {
        float totalDamage = 0f;
        List<float> allSuccessTouch = GameManager.instance.GetAllSuccessTouch();
        foreach (var success in allSuccessTouch)
        {
            totalDamage += playerData.damage * (success < 70 ? 1f :
                (success < 90) ? 1.5f :
               2f);
        }
        bossPattern.CurrentHealth -= totalDamage;
        BossLifeBarSmooth.DOValue(bossPattern.CurrentHealth / bossPattern.MaxHealth, 0.75f).Play();

        if (bossPattern.CurrentHealth < 0f)
            bossPattern.CurrentHealth = 0f;
        BossLifeBar.value = bossPattern.CurrentHealth / bossPattern.MaxHealth;
        if (bossPattern.CurrentHealth <= 0f)
        {
            GameManager.instance.bossObj.SetActive(false);
            gameObject.SetActive(false);
            return;
        }
        StartCoroutine(DelayBeforeNextTurn());
    }

IEnumerator DelayBeforeNextTurn()
    {
        GameManager.instance.RemoveAllSuccessTouch();
        yield return new WaitForSeconds(1f);
        bossPattern.LaunchPattern();
    }

    private void OnEnable()
    {
        capacityButtons[0].onClick.RemoveAllListeners();

        capacityButtons[0].onClick.AddListener(UseCapacity);

        BossLifeBar.value = 1f;
        BossLifeBarSmooth.value = 1f;
        TurnOrderIndex = 0;
        StartCoroutine(DelayBeforeNextTurn());
    }
}