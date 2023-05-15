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
        maxHealth = so.healthPoint * (1 + ((so.statModificatorValuePercentage / 100) * index));
        currentHealth = maxHealth;
        damage = so.damage * (1 + ((so.statModificatorValuePercentage / 100) * index));
        enemy = so;
        currentEnemyObj = Object.Instantiate(so.visual);
        enemyVFX = currentEnemyObj.GetComponent<EnemyVFX>();
    }
    
    public void InitCombat()
    {
        UIManager.instance.enemy.EnableEnemyHealth(true);
        UIManager.instance.enemy.enemyHealth.SetHealth(currentHealth, maxHealth);
        
        isActive = true;

        GameLoopManager.patternManager.StartPattern(enemy.patternSO);
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
        
        //GameLoopManager.instance.EndLevel();
    }

    public float getAttackData()
    {
        return damage;
    }
}
