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
    }

    public void Death()
    {
        GameLoopManager.instance.CheckForNextPattern();
    }
}
