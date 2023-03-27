using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class UI_Announcer : MonoBehaviour
{
    private TMP_Text tmpTxt => GetComponent<TMP_Text>();
    public float fadeIn;
    public float fadeOut;

    public void Announce(string text, Color textColor)
    {
        tmpTxt.text = text;
        tmpTxt.color = textColor;

        tmpTxt.DOFade(255, fadeIn).OnComplete(() => tmpTxt.DOFade(0, fadeOut));
    }
}
