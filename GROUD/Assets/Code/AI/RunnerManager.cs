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

    private void Update()
    {
        if (timer >= spawnFrequency)
        {
            SpawnObjectInRow();
            timer = 0;
        }
        else
        {
            if(!PatternManager.Instance.isTimelineActive) timer += Time.deltaTime;
        }
    }

    private void SpawnObjectInRow()
    {
        if(spawnPoints.Length == 0) return;
        
        int selectedWay = Helpers.GetRandomRange(0, spawnPoints.Length - 1);
        GameObject newObject = Instantiate(objectPrefab, spawnPoints[selectedWay].position, Quaternion.identity);
        newObject.transform.position += Vector3.up;
        
        ExperienceOrb objectOrb = newObject.GetComponent<ExperienceOrb>();
        objectOrb.orbPattern = patterns[Helpers.GetRandomRange(0, patterns.Length - 1)];
    }
    
}