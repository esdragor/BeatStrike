using System;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class GameManager : MonoBehaviour
{
    public GameState gameState = new GameState(GameState.LevelState.Exploration, GameState.TimeState.Play, GameState.EngineState.Menu);
    public static GameManager instance;
    public static Delegates.OnUpdated onUpdated;
    
    private CharacterInfos currentCharacterInfos;

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
        currentCharacterInfos.PowerInfo = _currentCharacterInfos.PowerInfo;
    }
}