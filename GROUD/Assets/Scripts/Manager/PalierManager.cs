using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public struct EnemyData
{
    public EnemySO enemy;
    public Material mat;
}

public class PalierManager : MonoBehaviour
{
    private static PalierManager instance;
    
    [SerializeField] private int indexPalier = 10;
    public int actualPalier = 0;
    [SerializeField] private EnemySO[] palierPrefabEnemies;
    private int indexEnemy = 0;

    private void Awake()
    {
        if (!instance) instance = this;
        else Destroy(this);
        if (PlayerPrefs.HasKey("Palier"))
            instance.actualPalier = PlayerPrefs.GetInt("Palier");
        else
            instance.actualPalier = 0;
        instance.indexEnemy = instance.actualPalier / instance.indexPalier;
        if (instance.indexEnemy >= instance.palierPrefabEnemies.Length)
            instance.indexEnemy = 0;
    }

    public static string GetPalierText()
    {
        return (instance.actualPalier > 0) ? instance.actualPalier.ToString() : "1";
    }

    public static void NewPalier(int indexPalier)
    {
        instance.actualPalier += indexPalier;
        PlayerPrefs.SetInt("Palier", instance.actualPalier);
        instance.indexEnemy++;
        if (instance.indexEnemy >= instance.palierPrefabEnemies.Length)
            instance.indexEnemy = 0;
        GameManager.instance.SetRandomBPM();
    }
    
    public static int GetIndexPalier()
    {
        return instance.indexPalier;
    }
    
    public static int GetActualPalier()
    {
        return instance.actualPalier;
    }
    
    public static EnemyData GetEnemy()
    {
        EnemyData data = new EnemyData();
        data.enemy = instance.palierPrefabEnemies[instance.indexEnemy];
        data.mat = data.enemy.material[Random.Range(0, data.enemy.material.Length)];
        
        return data;
    }
}