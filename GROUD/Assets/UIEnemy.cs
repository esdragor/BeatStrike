using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIEnemy : MonoBehaviour
{
    public Image healthImage;
    public TMP_Text healthText;
    
    public void UpdateHealthUI(float healthAmount, float healthMax)
    {
        Debug.Log("Update");
        healthImage.fillAmount = healthAmount / healthMax;
        healthText.text = $"{healthAmount}/{healthMax}";
    }
}
