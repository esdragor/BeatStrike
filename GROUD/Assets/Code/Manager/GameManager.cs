using System;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class GameManager : MonoBehaviour
{
    public GameState gameState = new GameState(GameState.LevelState.Exploration, GameState.TimeState.Play, GameState.EngineState.Menu);
    public static GameManager instance;
    public static Delegates.OnUpdated onUpdated;
    
    void Update()
    {
        onUpdated?.Invoke();
    }
    
    public float DistanceBetweenPoints = 15f;
    public Transform[] spawnPoints;
    [HideInInspector] public float timer;
    [HideInInspector] public bool isBossStarted;
    public GameObject bossObj;

    public BossFightManager BossFightManager;

    [SerializeField] private float speedBossArrived = 5f;


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
        bossObj.transform.position -= Vector3.forward * Time.deltaTime * speedBossArrived;
        if (bossObj.transform.position.z <= 30f)
        {
            onUpdated -= BossArriving;
            BossFightManager.gameObject.SetActive(true);
        }
    }


    public void StartBoss()
    {
        bossObj.SetActive(true);
        onUpdated += BossArriving;
        BossArrive = false;
        isBossStarted = true;
    }
}