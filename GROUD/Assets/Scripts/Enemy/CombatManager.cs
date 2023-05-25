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

    public void PreloadCombat()
    {
        index++;

        if (index > 0 && index % (PalierManager.GetIndexPalier() + 1) == 0) // new Palier
        {
            PalierManager.NewPalier();
        }

        EnemyData data = PalierManager.GetEnemy();


        float newMaxHealth = data.enemy.healthPoint;
        float newDamage = data.enemy.damage;
        float indexPalier = PalierManager.GetActualPalier() + index;

        for (int i = 1; i < indexPalier; i++)
        {
            newMaxHealth *= (1 + ((data.enemy.statModificatorValuePercentage / 100) * index));
            newDamage *= (1 + ((data.enemy.statModificatorValuePercentage / 100) * index));
        }
        maxHealth = newMaxHealth;
        currentHealth = maxHealth;
        damage = newDamage;
        enemy = data.enemy;
        if (currentEnemyObj) Object.Destroy(currentEnemyObj);
        currentEnemyObj = Object.Instantiate(enemy.visual);
        enemyVFX = currentEnemyObj.GetComponent<EnemyVFX>();
        currentEnemyObj.transform.position = GameLoopManager.instance.currentChunkLevelHeader.enemySpawnPoint.position;
        SkinnedMeshRenderer sk = currentEnemyObj.GetComponent<EnemyPrefab>().SkinnedMeshRenderer;
        for (int i = 0; i < sk.materials.Length; i++)
        {
            sk.materials[i] = data.mat;
        }
    }


    
    public void InitCombat()
    {
        UIManager.instance.enemy.EnableEnemyHealth(true);
        UIManager.instance.enemy.enemyHealth.SetHealth(currentHealth, maxHealth);
        GameLoopManager.instance.printDEFRoad(GameLoopManager.patternManager.StartPattern());
        isActive = true;
    }

    public void DealDamage(float amount)
    {
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

        isActive = false;

        GameLoopManager.instance.NextChunk();
    }

    public float getAttackData()
    {
        return damage;
    }
}