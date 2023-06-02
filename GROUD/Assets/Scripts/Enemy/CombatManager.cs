using System.Collections;
using UnityEditor;
using UnityEngine;

public class CombatManager
{
    public EnemyVFX enemyVFX;


    private float currentHealth;
    private float maxHealth;
    private float damage;
    private bool isActive;
    private EnemySO enemy;
    private GameObject currentEnemyObj;
    private int index = 0;
    
    private Animator currentEnemyAnimator;

    public void PreloadCombat()
    {
        index++;

        if (index > 0 && index % (PalierManager.GetIndexPalier() + 1) == 0) // new Palier
        {
            PalierManager.NewPalier();
        }

        EnemyData data = PalierManager.GetEnemy();


        float oldMaxHealth = data.enemy.healthPoint;
        float oldDamage = data.enemy.damage;

        float newMaxHealth = data.enemy.healthPoint;
        float newDamage = data.enemy.damage;
        float indexPalier = PalierManager.GetActualPalier() + index;

        for (int i = 1; i < indexPalier; i++)
        {
            newMaxHealth += ((data.enemy.statModificatorValuePercentage / 100) * oldMaxHealth);
            newDamage += ((data.enemy.statModificatorValuePercentage / 100) * oldDamage);
        }
        maxHealth = newMaxHealth;
        currentHealth = maxHealth;
        damage = newDamage;
        enemy = data.enemy;
        if (currentEnemyObj) Object.Destroy(currentEnemyObj);
        currentEnemyObj = Object.Instantiate(enemy.visual);
        enemyVFX = currentEnemyObj.GetComponent<EnemyVFX>();
        currentEnemyObj.transform.position = GameLoopManager.instance.currentChunkLevelHeader.enemySpawnPoint.position;
        EnemyPrefab sc = currentEnemyObj.GetComponent<EnemyPrefab>();
        currentEnemyAnimator = sc.animator;
        SkinnedMeshRenderer sk = sc.SkinnedMeshRenderer;
        for (int i = 0; i < sk.materials.Length; i++)
        {
            sk.materials[i] = data.mat;
        }
        sk.GetComponent<Renderer>().material = data.mat;
    }


    
    public void InitCombat(float timer, bool isStart)
    {
        UIManager.instance.enemy.EnableEnemyHealth(true);
        UIManager.instance.enemy.enemyHealth.SetHealth(currentHealth, maxHealth);
        GameLoopManager.instance.PrintComboRoad(GameLoopManager.patternManager.StartPattern(isStart, timer));
        isActive = true;
    }

    public void DealDamage(float amount)
    {
        currentEnemyAnimator.SetTrigger("TakeDamage");
        if (isActive)
        {
            currentHealth -= amount;

            if (currentHealth <= 0)
            {
                Death();
            }
        }

        UIManager.instance.enemy.enemyHealth.SetHealth(currentHealth, maxHealth);
    }

    private void Death()
    {
        UIManager.instance.enemy.EnableEnemyHealth(false);
        Object.Destroy(currentEnemyObj);
        PlayerManager.instance.vfxManager.PlaySFX("DeathEnemy");
        

        isActive = false;

        GameLoopManager.instance.NextChunk();
    }

    public float getAttackData()
    {
        return damage;
    }

    public void EnemyAttack()
    {
        currentEnemyAnimator.SetTrigger("Attack");
        PlayerManager.instance.vfxManager.PlaySFX("EnemyAttack");
    }
}