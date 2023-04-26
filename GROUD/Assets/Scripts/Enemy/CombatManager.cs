public class CombatManager
{
    private float currentHealth;
    private float maxHealth;
    
    private bool isActive;

    private EnemySO enemy;

    public void InitCombat(EnemySO enemySo)
    {
        currentHealth = enemySo.healthPoint;
        maxHealth = enemySo.healthPoint;
        enemy = enemySo;

        isActive = true;
        
        PlayerManager.instance.MovePlayerTo(GameLoopManager.instance.currentChunk.levelPos.position);
        UIManager.instance.enemy.EnableEnemyHealth(true);
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
        isActive = false;
        
        GameLoopManager.instance.EndLevel();
    }
}
