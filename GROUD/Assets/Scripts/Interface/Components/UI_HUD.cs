using DG.Tweening;
using TMPro;
using UnityEngine;

public class UI_HUD : MonoBehaviour
{
    public UI_PlayerHealth playerHealth;
    public TMP_Text textPalierInGame;

    [SerializeField] private RectTransform[] enemyTimeline;
    [SerializeField] private RectTransform playerTimeline;

    public void EnableHUD()
    {
        gameObject.SetActive(true);
    }

    public void DisableHUD()
    {
        gameObject.SetActive(false);
    }

    public void UpdateTimeLine(int index)
    {
        
        float duration = GameLoopManager.instance.GetVelocityTimerNewChunck();
        if (index < enemyTimeline.Length && index >= 0)
            playerTimeline.DOMoveX(enemyTimeline[index].position.x,
                GameLoopManager.instance.GetVelocityTimerNewChunck());
        else
            playerTimeline.position = enemyTimeline[0].position;
    }
}