using UnityEngine;
using Utilities;

public class GameManager : MonoBehaviour
{
    public GameState gameState = new GameState(Enums.LevelState.Exploration, Enums.TimeState.Play, Enums.EngineState.Menu);
    
    public static GameManager instance;
    public static Delegates.OnUpdated onUpdated;
    
    [HideInInspector] public CharacterInfos currentCharacterInfos;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);
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