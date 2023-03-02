using System;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static Delegates.OnUpdated onUpdated;

    public float DistanceBetweenPoints = 15f;
    public Transform[] spawnPoints;
    [HideInInspector] public float timer;
    [HideInInspector] public bool isBossStarted;
    public GameObject bossObj;

    public BossFightManager BossFightManager;

    [SerializeField] private float speedArrived = 5f;


    private bool BossArrive = false;
    private List<float> SuccessTouch = new List<float>();


    private void Awake()
    {
        instance = this;
    }

    public void AddSuccessTouch(float successTouch)
    {
        SuccessTouch.Add(successTouch);
    }
    
    public void RemoveAllSuccessTouch()
    {
        SuccessTouch.Clear();
    }
    
    public List<float> GetAllSuccessTouch()
    {
        return SuccessTouch;
    }

    private void BossArriving()
    {
        bossObj.transform.position -= Vector3.forward * Time.deltaTime * speedArrived;
        if (bossObj.transform.position.z <= 30f)
        {
            onUpdated -= BossArriving;
            BossFightManager.gameObject.SetActive(true);

        }
    }

    void Update()
    {
        onUpdated?.Invoke();
    }

    public void StartBoss()
    {
        bossObj.SetActive(true);
        onUpdated += BossArriving;
        BossArrive = false;
        isBossStarted = true;
    }
}