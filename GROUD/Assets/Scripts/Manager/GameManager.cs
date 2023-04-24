using Code.Player;
using UnityEngine;
using Utilities;

public class GameManager : MonoBehaviour
{
    public static GameState gameState = new (Enums.LevelState.Exploration, Enums.TimeState.Play, Enums.EngineState.Menu);
    
    public static GameManager instance;
    public static Delegates.OnUpdated onUpdated;
    
    public Power power;
    [HideInInspector] public Power currentPower;
    public float MovementRatioOk = 1f;
    public float MovementRatioGood = 1.5f;
    public float MovementRatioPerfect = 1f;
    
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);
        currentPower = new JustPerfect();
    }

    void Update()
    {
        onUpdated?.Invoke();
    }

    public void SetPlayerStats(Power _power)
    {
        currentPower = _power;
    }
}