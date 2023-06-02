using System;
using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] private int[] listOfBPMEasy = { 60, 120, 180, 240, 300 };
    [SerializeField] private int[] listOfBPMMedium = { 60, 120, 180, 240, 300 };
    [SerializeField] private int[] listOfBPMHard = { 60, 120, 180, 240, 300 };

    private float tickRate;
    private float tickTimer;
    private float BPM = 60;


    public CharacterInfos CharacterInfosPrefab;
    public CharacterInfos currentCharacterInfos;
    [HideInInspector] public bool bpmIsRandoming = false;
    [HideInInspector] public float savedTick = 0f;

    public float timeToShopReset = 5;
    private string lastDateKey = "LastOperationDate";
    private DateTime lastDate;
    private float fakeDeltaTime = -1f;

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

    public int[] GetBPMList(byte typeOf)
    {
        switch (typeOf)
        {
            case 0:
                return listOfBPMEasy;
            case 1:
                return listOfBPMMedium;
            case 2:
                return listOfBPMHard;
        }

        return listOfBPMHard;
    }

    void SaveReloadTime()
    {
        // actualise la date sauvegardée dans PlayerPrefs
        lastDate = DateTime.Now;
        PlayerPrefs.SetString(lastDateKey, lastDate.ToString());
        PlayerPrefs.Save();
    }

    public void RecheckEquipment()
    {
        for (int i = 0; i < currentCharacterInfos.equipment.Length; i++)
        {
            if (currentCharacterInfos.equipment[i] != null)
            {
                currentCharacterInfos.equipment[i].OnEquip = true;
            }
        }
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
        StartCoroutine(CalcAverageDeltaTime());
        //currentCharacterInfos.ResetCH();
    }

    void Update()
    {
        onUpdated?.Invoke();
        if (!gameState.IsTimePause())
            onUpdatedFrame?.Invoke();

        if (tickTimer >= 1)
        {
            OnTick?.Invoke();
            tickTimer = 0;
        }
        else
        {
            float value = (fakeDeltaTime > 0f ? fakeDeltaTime : Time.deltaTime) * tickRate;
            //float value = Time.deltaTime * tickRate;
            tickTimer += value;
            GameLoopManager.instance.AddTickCount(value);
        }
    }

    private IEnumerator CalcAverageDeltaTime()
    {
        float sum = 0f;

        for (int i = 0; i < 10; i++)
        {
            sum += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        sum /= 10f;
        fakeDeltaTime = sum;
        fakeDeltaTime *= 0.1f;
    }

    public void SetBPM(int BPM)
    {
        this.BPM = BPM;
        CalculateTickRate();
    }

    private IEnumerator AnimationBPM(int[] listOfBPM)
    {
        float nbSecond = 2f;
        int index = -1;
        UIManager.instance.banditManchot.SetActive(true);
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
        UIManager.instance.banditManchot.SetActive(false);
        SoundManager.PlayRandomBackground((int)Bpm);
        bpmIsRandoming = false;
    }

    public void SetRandomBPM(int[] listOfBPM)
    {
        bpmIsRandoming = true;
        
        int index = Random.Range(0, listOfBPM.Length);
        StartCoroutine(AnimationBPM(listOfBPM));
        SetBPM(listOfBPM[index]);
    }

    public float GetTickRate()
    {
        return tickRate;
    }

    void CalculateTickRate()
    {
        tickRate = Bpm / 60f;
    }
}