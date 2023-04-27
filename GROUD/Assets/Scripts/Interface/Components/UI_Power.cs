using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Power : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    public void SetText(InteractionSuccess succesOrFail)
    {
        text.text = "Power:\n";
        List<ScreenListener.SwipeDirection> remainingCombo = PlayerManager.instance.GetCurrentPower().power.RemainingCombo();
        foreach (var combo in remainingCombo)
        {
            text.text += combo + "\n";
        }
    }
}