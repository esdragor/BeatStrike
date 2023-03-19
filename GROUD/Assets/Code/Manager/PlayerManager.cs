using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    
    public float healthPoint;
    
    public PlayerLevelingData playerLevelingData;
    public float experience;
    public int level = 1;

    private void Awake()
    {
        if (instance == null) instance = this;
    } 

    public void AddExperience(float amount)
    {
        experience += amount;
        CheckForLevelUp();
    }

    void CheckForLevelUp()
    {
        if(level >= playerLevelingData.experienceTable.Length -1) return;
        
        if (experience >= playerLevelingData.experienceTable[level])
        {
            float rest = experience - playerLevelingData.experienceTable[level];
            level++;
            experience = rest;
        }
    }

    public void TakeDamage(float amount)
    {
        healthPoint -= amount;

        if (healthPoint <= 0)
        {
            Debug.Log("Player is dead");
        }
    }
}
