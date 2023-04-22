using UnityEngine;
using Utilities;

public class GameManager : MonoBehaviour
{
    public GameState gameState = new GameState(Enums.LevelState.Exploration, Enums.TimeState.Play, Enums.EngineState.Menu);
    
    public static GameManager instance;
    public static Delegates.OnUpdated onUpdated;
    
    public CharacterInfos CharacterInfosPrefab;
    [HideInInspector] public CharacterInfos currentCharacterInfos;
    public float MovementRatioOk = 1f;
    public float MovementRatioGood = 1.5f;
    public float MovementRatioPerfect = 1f;

    public DetectorVisual detectorVisual;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);
        currentCharacterInfos = ScriptableObject.CreateInstance<CharacterInfos>();
        currentCharacterInfos.SetPlayerStats(CharacterInfosPrefab);
        currentCharacterInfos.power = new JustPerfect();
    }

    void Update()
    {
        onUpdated?.Invoke();
    }

    public void SetPlayerStats(CharacterInfos _currentCharacterInfos)
    {
        currentCharacterInfos = ScriptableObject.CreateInstance<CharacterInfos>();
        currentCharacterInfos.playerStats = _currentCharacterInfos.playerStats;
        currentCharacterInfos.playerSprite = _currentCharacterInfos.playerSprite;
        currentCharacterInfos.power = _currentCharacterInfos.power;
    }
}