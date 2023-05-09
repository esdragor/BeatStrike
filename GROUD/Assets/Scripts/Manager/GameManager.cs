using System;
using Code.Player;
using UnityEngine;
using Utilities;

public class GameManager : MonoBehaviour
{
    public static GameState gameState = new (Enums.LevelState.Exploration, Enums.TimeState.Play, Enums.EngineState.Menu);
    public static DatabaseManager databaseManager;
    
    public static GameManager instance;
    public static Delegates.OnUpdated onUpdated;
    public static Action OnTick;
    private float tickRate;
    private float tickTimer;
    private float BPM = 60;
    
    public CharacterInfos currentCharacterInfos;
    
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);
        
        databaseManager = new DatabaseManager();
        currentCharacterInfos.ResetCH();
            
        CalculateTickRate();
    }

    void Update()
    {
        onUpdated?.Invoke();
        
        if (tickTimer >= 1)
        {
            OnTick?.Invoke();
            tickTimer = 0;
        }
        else
        {
            tickTimer += Time.deltaTime * tickRate;
        }
    }

    public void SetBPM(int BPM)
    {
        this.BPM = BPM;
        CalculateTickRate();
    }
    void CalculateTickRate() => tickRate = BPM / 60f;
}