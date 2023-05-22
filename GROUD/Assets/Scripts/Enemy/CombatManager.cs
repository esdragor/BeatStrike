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
    private int index = -1;


    public void PreloadCombat(EnemySO so)
    {
        index++;

        if (index > 0 && index % PalierManager.GetIndexPalier() == 0) // new Palier
        {
            PalierManager.NewPalier(PalierManager.GetIndexPalier());
        }

        maxHealth = so.healthPoint * (1 + ((so.statModificatorValuePercentage / 100) * index));
        currentHealth = maxHealth;
        damage = so.damage * (1 + ((so.statModificatorValuePercentage / 100) * index));
        enemy = so;
        if (currentEnemyObj) Object.Destroy(currentEnemyObj);
        currentEnemyObj = Object.Instantiate(so.visual);
        enemyVFX = currentEnemyObj.GetComponent<EnemyVFX>();
        currentEnemyObj.transform.position = GameLoopManager.instance.currentChunk.enemySpawnPoint.position;
    }

    public void InitCombat()
    {
        UIManager.instance.enemy.EnableEnemyHealth(true);
        UIManager.instance.enemy.enemyHealth.SetHealth(currentHealth, maxHealth);

        isActive = true;

        GameLoopManager.instance.printDEFRoad(GameLoopManager.patternManager.StartPattern());
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