using System;
using System.Collections;
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
    public bool bpmIsRandoming = false;

    public float timeToShopReset = 5;
    private string lastDateKey = "LastOperationDate";
    private DateTime lastDate;

    public float Bpm => BPM;

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
        //SetRandomBPM();
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

    private IEnumerator AnimationBPM()
    {
        float nbSecond = 2f;
        int index = -1;
        while (nbSecond > 0f)
        {
            nbSecond -= Time.deltaTime;
            
            index++;
            if (index >= listOfBPM.Length)
            {
                index = 0;
            }
            UIManager.instance.debugBanditBPM.text = "BPM : " + listOfBPM[index];
            yield return new WaitForEndOfFrame();
        }
        UIManager.instance.debugBanditBPM.text = "BPM : " + Bpm;
        yield return new WaitForSeconds(2f);
        UIManager.instance.debugBanditBPM.text = "";
        SoundManager.PlayRandomBackground((int)Bpm);
        bpmIsRandoming = false;
    }

    public void SetRandomBPM()
    {
        bpmIsRandoming = true;
        int index = Random.Range(0, listOfBPM.Length);
        StartCoroutine(AnimationBPM());
        SetBPM(listOfBPM[index]);
    }

    void CalculateTickRate()
    {
       tickRate = Bpm / 60f;
    }
}