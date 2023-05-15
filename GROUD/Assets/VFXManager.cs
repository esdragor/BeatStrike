using UnityEngine;

public class VFXManager : MonoBehaviour
{
    [Header("Road")]
    public ParticleSystem missFX;
    public ParticleSystem okFX;
    public ParticleSystem greatFX;
    public ParticleSystem perfectFX;
    public ParticleSystem rDodgeFX;
    public ParticleSystem rAttackFX;

    [Header("Player")] 
    public ParticleSystem attackFX;
    public ParticleSystem dodgeFX;
    public ParticleSystem hurtFX;

    private ParticleSystem punchD => GameLoopManager.combatManager.enemyVFX.punchD;
    private ParticleSystem punchL => GameLoopManager.combatManager.enemyVFX.punchL;
    private ParticleSystem punchR => GameLoopManager.combatManager.enemyVFX.punchR;
    private ParticleSystem punchU => GameLoopManager.combatManager.enemyVFX.punchU;
    private ParticleSystem ability => GameLoopManager.combatManager.enemyVFX.ability;

    public void PlaySFX(string sName, ScreenListener.SwipeDirection dir = ScreenListener.SwipeDirection.NULL)
    {
        switch (sName)
        {
            case "Miss":
                missFX.Play();
                break;
            case "Ok":
                okFX.Play();
                break;
            case "Great":
                greatFX.Play();
                break;
            case "Perfect":
                perfectFX.Play();
                break;

            case "Dodge":
                rDodgeFX.Play();
                break;
            case "Attack":
                rAttackFX.Play();

                switch (dir)
                {
                    case ScreenListener.SwipeDirection.UP:
                        punchU.Play();
                        break;
                    
                    case ScreenListener.SwipeDirection.DOWN:
                        punchD.Play();
                        break;
                    
                    case ScreenListener.SwipeDirection.LEFT:
                        punchL.Play();
                        break;
                    
                    case ScreenListener.SwipeDirection.RIGHT:
                        punchR.Play();
                        break;
                }
                break;
            case "Hurt":
                hurtFX.Play();
                break;
            
            case "Ability":
                ability.Play();
                break;
        }
    }
}
