using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    [SerializeField] private ImageEffect cam;

    [Header("Road")] [SerializeField] private ParticleSystem missFX;
    [SerializeField] private ParticleSystem okFX;
    [SerializeField] private ParticleSystem greatFX;
    [SerializeField] private ParticleSystem perfectFX;
    [SerializeField] private ParticleSystem rDodgeFX;
    [SerializeField] private ParticleSystem rAttackFX;
    [SerializeField] private ParticleSystem GoodComboFX;
    [SerializeField] private ParticleSystem FailComboFX;
    [SerializeField] private ParticleSystem ReadyComboFX;
    [SerializeField] private ParticleSystem DeathEnemyFX;

    [SerializeField] private ParticleSystem PhaseAnnouncerVFX;

    [Header("Player")] [SerializeField] private ParticleSystem attackFX;
    [SerializeField] private ParticleSystem dodgeFX;
    [SerializeField] private ParticleSystem hurtFX;
    [Header("Enemy")] [SerializeField] private ParticleSystem hurtEnemyFX;

    private ParticleSystem punchD => GameLoopManager.combatManager.enemyVFX.punchD;
    private ParticleSystem punchL => GameLoopManager.combatManager.enemyVFX.punchL;
    private ParticleSystem punchR => GameLoopManager.combatManager.enemyVFX.punchR;
    private ParticleSystem punchU => GameLoopManager.combatManager.enemyVFX.punchU;
    private ParticleSystem ability => GameLoopManager.combatManager.enemyVFX.ability;
    private ParticleSystem attackEnemy => GameLoopManager.combatManager.enemyVFX.attack;
    private float attackDelayEnemy => GameLoopManager.combatManager.enemyVFX.attackDelay;
    private ParticleSystem hitEnemy => GameLoopManager.combatManager.enemyVFX.hit;

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
                StartCoroutine(launchWithDeadTime(hitEnemy, 0.8f));
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
            case "EnemyAttack":
                if (attackEnemy)
                {
                    LaunchFXWithDelay(attackEnemy, attackDelayEnemy);
                }

                break;
            case "GCombo":
                NotReadyCombo();
                GoodComboFX.Play();
                break;
            case "FailCombo":
                NotReadyCombo();
                FailComboFX.Play();
                break;
            case "LastCombo":
                ReadyComboFX.gameObject.SetActive(true);
                ReadyComboFX.Play();
                break;
            case "DeathEnemy":
                DeathEnemyFX.transform.position =
                    GameLoopManager.combatManager.enemyVFX.gameObject.transform.GetChild(1).position;
                DeathEnemyFX.Play();
                break;
        }
    }

    public async void LaunchFXWithDelay(ParticleSystem fx, float delay)
    {
        await Task.Delay((int)(delay * 1000));
        if (fx)
            fx.Stop();
        if (fx)
            fx.Play();
    }

    public async void AnnouncerPhaseVFX(bool isDef)
    {
        PhaseAnnouncerVFX.GetComponent<Renderer>().material.SetInt("_isDef", isDef ? 1 : 0);
        PhaseAnnouncerVFX.Play();
        UIManager.instance.hud.DisableHUD();

        while (PhaseAnnouncerVFX.isPlaying)
        {
            await Task.Yield();
        }

        UIManager.instance.hud.EnableHUD();
    }

    IEnumerator launchWithDeadTime(ParticleSystem ps, float time)
    {
        if (ps)
            ps.Play();
        yield return new WaitForSeconds(time);
        if (ps)
            ps.Stop();
    }

    public void NotReadyCombo()
    {
        ReadyComboFX.gameObject.SetActive(false);
    }
}