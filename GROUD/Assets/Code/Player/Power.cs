using System;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class Power
{
    [SerializeField] private float powerCooldown;
    [SerializeField] private float ratioPerfect = 2.5f;
    [SerializeField] private float ratioGood = 1.75f;
    [SerializeField] private float ratioOk = 1f;
    [SerializeField] private float amountCooldown = 1f;

    private bool onCooldown = false;
    private float currentCooldown = 0;

    private async void JustPerfect()
    {
        float waitDuration = GameManager.instance.currentCharacterInfos.playerStats.competenceDuration * 1000;

        PlayerManager.instance.powerIsRunning = true;
        await Task.Delay((int)waitDuration);
        PlayerManager.instance.powerIsRunning = false;
        currentCooldown = powerCooldown;
    }

    public void ModifyCooldown(InteractionSuccess success)
    {
        if (currentCooldown <= 0) return;
        float amount = amountCooldown;
        switch (success)
        {
            case InteractionSuccess.Perfect:
                amount *= ratioPerfect;
                break;
            case InteractionSuccess.Good:
                amount *= ratioGood;
                break;
            case InteractionSuccess.Ok:
                amount *= ratioOk;
                break;
        }

        currentCooldown -= amount;
        if (currentCooldown <= 0)
            onCooldown = false;

        if (currentCooldown <= 0)
            PlayerManager.instance.CDPowerImage.fillAmount = 1;
        else
            PlayerManager.instance.CDPowerImage.fillAmount = currentCooldown / powerCooldown;
    }

    public void Execute()
    {
        if (onCooldown) return;
        onCooldown = true;
        JustPerfect();
    }
}