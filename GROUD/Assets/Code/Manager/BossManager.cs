using System;
using UnityEngine;
using Utilities;

public class BossManager : MonoBehaviour
{
    public static BossManager instance;

    public GameObject bossObj;
    public float speed;
    
    private BossLevelRound bossPattern;
    
    public float bossHealthPoint;
    public float damagePool;

    private Action onBossDead;

    private void Awake()
    {
        if (instance == null) instance = this;
    } 
    
    public void StartBossFight(BossLevelRound p)
    {
        
        onBossDead += OnBossDead;
        PatternManager.OnPatternEnd += ReleaseDamage;

        bossObj.SetActive(true);
        GameManager.onUpdated += BossArrival;
    }

    void BossArrival()
    {
        bossObj.transform.position -= Vector3.forward * Time.deltaTime * speed;
        if (bossObj.transform.position.z <= 30f)
        {
            GameManager.instance.gameState.SwitchLevelState(Enums.LevelState.Boss);
            GameManager.onUpdated -= BossArrival;
            LevelManager.instance.PlayPattern();
        }
    }
    
    public void AddDamageToPool(float amount)
    {
        damagePool += amount;
    }

    private void ReleaseDamage()
    {
        bossHealthPoint -= damagePool;
        damagePool = 0;
        CheckState();
    }

    void CheckState()
    {
        if (bossHealthPoint <= 0)
        {
            onBossDead?.Invoke();
        }
    }

    void OnBossDead()
    {        
        PatternManager.OnPatternEnd -= ReleaseDamage;
        PatternManager.Instance.ForceEnd();
        bossObj.SetActive(false);
        onBossDead -= OnBossDead;
        
        GameManager.instance.gameState.SwitchLevelState(Enums.LevelState.Exploration);
    }
}
