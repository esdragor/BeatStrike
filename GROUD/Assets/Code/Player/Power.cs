using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Power
{
    public float duration = 10f;
    
    async void isImmortal()
    {
        float Waitduration = duration * 1000;
        
        PlayerManager.instance.immortality = true;
        await Task.Delay((int)Waitduration);
        PlayerManager.instance.immortality = false;
    }
    
    public void Execute()
    {
        isImmortal();
    }
}