using UnityEngine;

public class VFXManager : MonoBehaviour
{
    [SerializeField] private ImageEffect cam;

    [Header("Road")]
    [SerializeField] private ParticleSystem missFX;
    [SerializeField] private ParticleSystem okFX;
    [SerializeField] private ParticleSystem greatFX;
    [SerializeField] private ParticleSystem perfectFX;
    [SerializeField] private ParticleSystem rDodgeFX;
    [SerializeField] private ParticleSystem rAttackFX;
    [SerializeField] private ParticleSystem GoodComboFX;

    [Header("Player")] 
    [SerializeField] private ParticleSystem attackFX;
    [SerializeField] private ParticleSystem dodgeFX;
    [SerializeField] private ParticleSystem hurtFX;
    [Header("Enemy")] 
    [SerializeField] private ParticleSystem hurtEnemyFX;

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
                cam.effID = 1;
                hurtFX.Play();
                break;
            case "HurtEnemy":
                cam.effID = 0;
                hurtEnemyFX.Play();
                break;
            
            case "Ability":
                ability.Play();
                break;
            case "GCombo":
                GoodComboFX.Play();
                break;
        }
    }
}
