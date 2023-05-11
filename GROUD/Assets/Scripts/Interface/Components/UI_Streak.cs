using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Streak : MonoBehaviour
{
    [SerializeField] private TMP_Text textStreak;
    [SerializeField] private Transform sliderStreak;
    [SerializeField] private GameObject prefabSlider;
    [SerializeField] private Image streakFill;
    
    private List<Image> sliders = new List<Image>();

    [SerializeField] private float punchScale;
    [SerializeField] private float punchDuration;
    [SerializeField] private float fillDuration;
    private Vector3 punchVector => new Vector3(punchScale, punchScale, punchScale);
    
    public void Enable()
    {
        int multiplier = StreakManager.GetNbNeedToMultiply();

        for (int i = 0; i < multiplier; i++)
        {
           sliders.Add(Instantiate(prefabSlider, sliderStreak).GetComponent<Image>());
        }

        UpdateStreakAndMultiplier();
    }

    private int multiplierBackup;
    public void UpdateStreakAndMultiplier()
    {
        textStreak.text = $"X{StreakManager.GetMultiplier()}";

        if (StreakManager.GetMultiplier() != multiplierBackup)
        {
            transform.DOPunchScale(punchVector, punchDuration);
        }

        multiplierBackup = StreakManager.GetMultiplier();
        streakFill.DOFillAmount(StreakManager.GetCurrentLoadBarPercentage(), fillDuration); 
        
        foreach (var slider in sliders)
        {
            slider.color = Color.black;
        }
        int multiplier = StreakManager.GetCurrentLoadBar();
        for (int i = 0; i < multiplier; i++)
        {
            sliders[i].color = Color.white;
        }
    }
}
