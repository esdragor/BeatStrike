using System;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class Power
{
    private async void JustPerfect()
    {
        float waitDuration = GameManager.instance.currentCharacterInfos.playerStats.competenceDuration * 1000;
        
        PlayerManager.instance.powerIsRunning = true;
        await Task.Delay((int)waitDuration);
        PlayerManager.instance.powerIsRunning = false;
    }
    
    public void Execute()
    {
        JustPerfect();
    }
}