using System;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class Power
{
    [SerializeField] private float duration = 10f;

    private async void IsImmortal()
    {
        float waitDuration = duration * 1000;
        
        PlayerManager.instance.powerIsRunning = true;
        await Task.Delay((int)waitDuration);
        PlayerManager.instance.powerIsRunning = false;
    }
    
    public void Execute()
    {
        IsImmortal();
    }
}