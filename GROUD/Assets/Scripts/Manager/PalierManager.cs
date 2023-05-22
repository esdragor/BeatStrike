using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


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
        
        UIManager.instance.announcer.Announce($"PALIER {instance.actualPalier}-{instance.indexEnemy + 1}", Color.white);
    }
    
    public static int GetIndexPalier()
    {
        return instance.indexPalier;
    }
    
    public static GameObject GetEnemy()
    {
        EnemySO enemy = instance.palierPrefabEnemies[instance.indexEnemy];
        GameObject go = Instantiate(enemy.visual);
        go.GetComponent<Renderer>().material = enemy.material[Random.Range(0, enemy.material.Length)];
        
        return go;
    }
}