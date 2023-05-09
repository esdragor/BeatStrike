using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    [Header("Road")]
    public ParticleSystem missFX;
    public ParticleSystem okFX;
    public ParticleSystem greatFX;
    public ParticleSystem perfectFX;

    [Header("Player")] 
    public ParticleSystem dodgeFX;
    public ParticleSystem attackFX;

    public void PlaySFX(string sName)
    {
        switch (sName)
        {
            case "Miss": missFX.Play(); break;
            case "Ok" : okFX.Play(); break;
            case "Great" : greatFX.Play(); break;
            case "Perfect" : perfectFX.Play(); break;
            
            case "Dodge" : dodgeFX.Play(); break;
            case "Attack" : attackFX.Play(); break;
        }
    }
}
