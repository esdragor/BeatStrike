using UnityEngine;
using Utilities;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static Delegates.OnUpdated onUpdated;

    public float timer;
    public float timeToBoss = 20f;
    public bool isBossStarted;
    
    public BossFightManager BossFightManager;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        onUpdated?.Invoke();
        timer += Time.deltaTime;

        if (timer > timeToBoss && !isBossStarted && !PatternManager.Instance.isTimelineActive)
        {
            StartBoss();
        }
    }

    void StartBoss()
    {
        BossFightManager.gameObject.SetActive(true);
        isBossStarted = true;
    }
    
}
