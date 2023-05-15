using System;
using System.Globalization;
using UnityEngine;
using Utilities;

public class GameManager : MonoBehaviour
{
    public static GameState gameState = new(Enums.LevelState.Exploration, Enums.TimeState.Play, Enums.EngineState.Menu);
    public static DatabaseManager databaseManager;

    public static GameManager instance;
    public static Delegates.OnUpdated onUpdated;
    public static Action OnTick;
    private float tickRate;
    private float tickTimer;
    private float BPM = 60;

    public CharacterInfos currentCharacterInfos;

    public float timeToShopReset = 5;
    private string lastDateKey = "LastOperationDate";
    private DateTime lastDate;
    private int actualPalier = 0;

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

    public void NewPalier(int indexPalier)
    {
        actualPalier += indexPalier;
        PlayerPrefs.SetInt("Palier", actualPalier);
    }

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        databaseManager = new DatabaseManager();
        currentCharacterInfos.ResetCH();

        gameState.SwitchEngineState(Enums.EngineState.Menu);
        if (PlayerPrefs.HasKey("Palier"))
            actualPalier = PlayerPrefs.GetInt("Palier");
        else
            actualPalier = 0;
        CalculateTickRate();
    }
    
    public string GetPalierText()
    {
        return (actualPalier > 0) ? actualPalier.ToString() : "1";
    }

    private void Start()
    {
        string lastDateString = PlayerPrefs.GetString(lastDateKey, DateTime.Now.ToString());
        if (DateTime.TryParse(lastDateString, out lastDate) == false)
        {
            lastDate = DateTime.MinValue;
        }
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