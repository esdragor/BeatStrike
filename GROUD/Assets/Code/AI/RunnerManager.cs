using System;
using Code.AI;
using UnityEngine;
using Utilities;

public class RunnerManager : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject objectPrefab;
    
    public float spawnFrequency;
    public float timer;
    
    public Pattern[] patterns;

    private void Awake()
    {
        PatternManager.OnPatternEnd += OnPatternEnd;
    }

    private void OnPatternEnd()
    {
        Debug.Log("Own Experience !");
    }

    private void Update()
    {
        if(GameManager.instance.isBossStarted) return;
        
        if (timer >= spawnFrequency)
        {
            SpawnObjectInRow();
            timer = 0;
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    private void SpawnObjectInRow()
    {
        if(spawnPoints.Length == 0) return;
        
        int selectedWay = Helpers.GetRandomRange(0, spawnPoints.Length - 1);
        GameObject newObject = PatternPoolManager.Instance.GetCircleFromPool();
        Vector3 spawnPosition = spawnPoints[selectedWay].position;
        newObject.transform.position = new Vector3(spawnPosition.x, 1, spawnPosition.z);
        
        ExperienceOrb objectOrb = newObject.GetComponent<ExperienceOrb>();
        Vector3 target = newObject.transform.position;
        target.z = 0;
        objectOrb.specialTargetPosition = target;
        Pattern selectedPattern = patterns[Helpers.GetRandomRange(0, patterns.Length - 1)];
        objectOrb.orbPattern = selectedPattern;
    }
    
}