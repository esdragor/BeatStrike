using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_EnemyHealth : MonoBehaviour
{
        public Image healthBar;
        public TMP_Text healthTxt;

        public void SetHealth(float currentHp, float maxHP)
        {
                healthBar.DOFillAmount(currentHp /maxHP , 1f).OnPlay(() => healthBar.rectTransform.DOShakePosition(1f, 3f));
                healthTxt.text = $"{Math.Round(currentHp, 0)}/{Math.Round(maxHP, 0)}";
        }
}