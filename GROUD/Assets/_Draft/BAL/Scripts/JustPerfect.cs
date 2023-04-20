using System;
using System.Threading.Tasks;
using Code.Player;
using UnityEngine;

[Serializable]
public class JustPerfect : Power
{
    [SerializeField] private float powerCooldown;
    [SerializeField] private float ratioPerfect = 2.5f;
    [SerializeField] private float ratioGood = 1.75f;
    [SerializeField] private float ratioOk = 1f;
    [SerializeField] private float amountCooldown = 1f;

    private bool onCooldown = false;
    private float currentCooldown = 0;
    private float amountCooldownPerSecond = 1000f;


    private async void JustPerfectAsync()
    {
        float waitDuration = amountCooldownPerSecond * 1000f;
        PlayerManager.instance.justPerfectEnabled = true;
        onCooldown = true;
        await Task.Delay((int)waitDuration);
        onCooldown = false;
        PlayerManager.instance.justPerfectEnabled = false;
    }

    public void ModifyCooldown(InteractionSuccess success)
    {
        // if (!onCooldown) return;
        // float amount = amountCooldown;
        // switch (success)
        // {
        //     case InteractionSuccess.Perfect:
        //         amount *= ratioPerfect;
        //         break;
        //     case InteractionSuccess.Good:
        //         amount *= ratioGood;
        //         break;
        //     case InteractionSuccess.Ok:
        //         amount *= ratioOk;
        //         break;
        // }

        // currentCooldown -= amount;
        // if (currentCooldown <= 0)
        //     onCooldown = false;
        //
        // if (currentCooldown <= 0)
        //     PlayerManager.instance.CDPowerImage.fillAmount = 1;
        // else
        //     PlayerManager.instance.CDPowerImage.fillAmount = currentCooldown / powerCooldown;
    }

    public override void OnSet()
    {
        //PlayerManager.onInteractionSuccess += ModifyCooldown;
    }

    public override void OnUnset()
    {
        //PlayerManager.onInteractionSuccess -= ModifyCooldown;
    }

    public override void Execute()
    {
        if (onCooldown) return;
        JustPerfectAsync();
    }
}