using UnityEngine;

public class CombatManager
{
    private float currentHealth;
    private float maxHealth;
    
    private bool isActive;

    private EnemySO enemy;
    private GameObject currentEnemyObj;

    public void PreloadCombat(EnemySO so)
    {
        currentHealth = so.healthPoint;
        maxHealth = so.healthPoint;
        enemy = so;
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
        
        GameLoopManager.instance.EndLevel();
    }
}
