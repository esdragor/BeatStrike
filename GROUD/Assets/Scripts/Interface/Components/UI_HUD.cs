using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class UI_HUD : MonoBehaviour
{
    public UI_PlayerHealth playerHealth;
    public TMP_Text textPalierInGame;

    [SerializeField] private RectTransform[] enemyTimeline;
    [SerializeField] private RectTransform playerTimeline;
    [SerializeField] private RectTransform TextKey;

    public void EnableHUD()
    {
        gameObject.SetActive(true);
    }

    public void DisableHUD()
    {
        gameObject.SetActive(false);
    }

    public void UpdateTimeLine(int index, float duration = 1)
    {
        if (duration == 1)
        {
            duration = GameLoopManager.instance.GetVelocityTimerNewChunck();
        }
         
        if (index < enemyTimeline.Length && index > 0)
            playerTimeline.DOMoveX(enemyTimeline[index].position.x,
                GameLoopManager.instance.GetVelocityTimerNewChunck());
        else
            playerTimeline.position = enemyTimeline[0].position;
    }

    
   private IEnumerator printKey()
    {
        TextKey.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        TextKey.gameObject.SetActive(false);
    }
    
    public void PrintGainKey()
    {
        StartCoroutine(printKey());
    }
}