using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Streak : MonoBehaviour
{
    [SerializeField] private TMP_Text textStreak;
    [SerializeField] private Transform sliderStreak;
    [SerializeField] private GameObject prefabSlider;
    
    private List<Image> sliders = new List<Image>();

    public void Enable()
    {
        int multiplier = StreakManager.GetNbNeedToMultiply();

        for (int i = 0; i < multiplier; i++)
        {
           sliders.Add(Instantiate(prefabSlider, sliderStreak).GetComponent<Image>());
        }
    }

    public void UpdateStreakAndMultiplier()
    {
        textStreak.text = $"X{StreakManager.GetMultiplier()}";
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
