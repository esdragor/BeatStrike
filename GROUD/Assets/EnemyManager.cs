using System;
using UnityEngine;
using Utilities;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    private LevelRoadManager _roadManager => FindObjectOfType<LevelRoadManager>();
    
    public EnemySO currentEnemy;
    public UIEnemy enemyInterface;
    public float currentHealth;
    private bool isActive;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void SetEnemy(EnemySO so)
    {
        currentEnemy = so;

        currentHealth = currentEnemy.healthPoint;
        
        isActive = true;
        
        enemyInterface.gameObject.SetActive(true);
        enemyInterface.UpdateHealthUI(currentHealth, currentEnemy.healthPoint);
    }
    
    public void GetHurt(float amount)
    {
        if (isActive)
        {
            currentHealth -= amount;
            Debug.Log("ENEMY HURT");
            enemyInterface.UpdateHealthUI(currentHealth, currentEnemy.healthPoint);
            
            if (currentHealth <= 0)
            {
                enemyInterface.gameObject.SetActive(false);
                _roadManager.CheckStepsToTarget(20);
                GameManager.instance.gameState.SwitchLevelState(Enums.LevelState.Exploration);
                isActive = false;
            }
        }
    }
}
