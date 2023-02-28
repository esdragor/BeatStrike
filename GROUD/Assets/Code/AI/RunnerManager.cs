using System;
using Code.AI;
using UnityEngine;
using Utilities;

public class RunnerManager : MonoBehaviour
{
    public Vector3 wayOnePosition;
    public Vector3 wayTwoPosition;
    public Vector3 wayThreePosition;
    
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
        GameObject newObject = Instantiate(objectPrefab, spawnPoints[selectedWay].position, Quaternion.identity);
        newObject.transform.position += Vector3.up;
        
        ExperienceOrb objectOrb = newObject.GetComponent<ExperienceOrb>();
        Pattern selectedPattern = patterns[Helpers.GetRandomRange(0, patterns.Length - 1)];
        objectOrb.orbPattern = selectedPattern;

        for (int i = 0; i < selectedPattern.interactions.Count; i++)
        {
            switch (selectedWay)
            {
                case 0:
                    selectedPattern.interactions[i].spawnPosition = wayOnePosition + new Vector3(0, Helpers.GetRandomRange(-500, 500),0);
                    break;
                
                case 1 :
                    selectedPattern.interactions[i].spawnPosition = wayTwoPosition + new Vector3(0, Helpers.GetRandomRange(-500, 500),0);
                    break;
                
                case 3:
                    selectedPattern.interactions[i].spawnPosition = wayThreePosition + new Vector3(0, Helpers.GetRandomRange(-500, 500),0);
                    break;
            }
        }
        
    }
    
}