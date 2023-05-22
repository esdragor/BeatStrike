using System;
using System.Globalization;
using UnityEngine;
using Utilities;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameState gameState = new(Enums.LevelState.Exploration, Enums.TimeState.Play, Enums.EngineState.Menu);
    public static DatabaseManager databaseManager;

    public static GameManager instance;
    public static Delegates.OnUpdated onUpdated;
    public static Delegates.OnUpdated onUpdatedFrame;
    public static Action OnTick;
    
    [SerializeField] private int[] listOfBPM = {60, 120, 180, 240, 300};
    
    private float tickRate;
    private float tickTimer;
    private float BPM = 60;

    
    public CharacterInfos CharacterInfosPrefab;
    public CharacterInfos currentCharacterInfos;

    public float timeToShopReset = 5;
    private string lastDateKey = "LastOperationDate";
    private DateTime lastDate;

    public double GetLastShopReload()
    {
        // calcule le temps restant en secondes
        TimeSpan timeSinceLastOperation = DateTime.Now - lastDate;
        float timeRemaining = timeToShopReset * 60 - (float)timeSinceLastOperation.TotalSeconds;

        if (timeRemaining < 0)
        {
            SaveReloadTime();
        }

        return timeRemaining;
    }

    void SaveReloadTime()
    {
        // actualise la date sauvegardée dans PlayerPrefs
        lastDate = DateTime.Now;
        PlayerPrefs.SetString(lastDateKey, lastDate.ToString());
        PlayerPrefs.Save();
    }

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        
        databaseManager = new DatabaseManager();

        gameState.SwitchEngineState(Enums.EngineState.Menu);
        CalculateTickRate();
    }

    private void Start()
    {
        string lastDateString = PlayerPrefs.GetString(lastDateKey, DateTime.Now.ToString());
        if (DateTime.TryParse(lastDateString, out lastDate) == false)
        {
            lastDate = DateTime.MinValue;
        }
        currentCharacterInfos = ScriptableObject.CreateInstance<CharacterInfos>();
        currentCharacterInfos.SetPlayerStats(CharacterInfosPrefab);
        Inventory.Init();
        //currentCharacterInfos.ResetCH();
    }

    void Update()
    {
        onUpdated?.Invoke();
        onUpdatedFrame?.Invoke();

        if (tickTimer >= 1)
        {
            OnTick?.Invoke();
            tickTimer = 0;
        }
        else
        {
            float value = Time.deltaTime * tickRate;
            tickTimer += value;
            GameLoopManager.instance.AddTickCount(value);
        }
    }

    public void SetBPM(int BPM)
    {
        this.BPM = BPM;
        CalculateTickRate();
    }

    void CalculateTickRate()
    {
       tickRate = BPM / 60f;
    }
}