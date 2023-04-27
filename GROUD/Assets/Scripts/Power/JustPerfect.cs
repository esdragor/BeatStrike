using System;
using System.Threading.Tasks;
using Code.Player;
using UnityEngine;

[Serializable]
/*public class JustPerfect : Power
{
     [SerializeField] private float ratioPerfect = 2.5f;
     [SerializeField] private float ratioGood = 1.75f;
     [SerializeField] private float ratioOk = 1f;
     [SerializeField] private float amountCooldown = 1f;
    
    private float currentCooldown = 0;
    private float amountCooldownPerSecond = 1000f;


    private async void JustPerfectAsync()
    {
        currentCooldown = powerCooldown;
        float waitDuration = amountCooldownPerSecond * 1000f;
        PlayerManager.instance.justPerfectEnabled = true;
        powerRunning = true;
        await Task.Delay((int)waitDuration);
        powerRunning = false;
        PlayerManager.instance.justPerfectEnabled = false;
    }

    public void ModifyCooldown(InteractionSuccess success)
    {
        if (powerRunning) return;

        currentCooldown -= ratioCD;
        if (currentCooldown <= 0)
            onCooldown = false;
        
        if (currentCooldown <= 0)
            PlayerManager.instance.CDPowerImage.fillAmount = 1;
        else
            PlayerManager.instance.CDPowerImage.fillAmount = currentCooldown / powerCooldown;
    }

    public override void OnSet()
    {
        PlayerManager.onInteractionSuccess += ModifyCooldown;
        OnSetBase();
    }

    public override void OnUnset()
    {
        PlayerManager.onInteractionSuccess -= ModifyCooldown;
    }

    public override void Execute()
    {
        if (onCooldown || powerRunning) return;
        JustPerfectAsync();
    }
}*/

public class JustPerfect
{
    
}