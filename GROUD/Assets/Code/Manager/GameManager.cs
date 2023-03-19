using System;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class GameManager : MonoBehaviour
{
    public GameState gameState = new GameState(GameState.LevelState.Exploration, GameState.TimeState.Play, GameState.EngineState.Menu);
    public static GameManager instance;
    public static Delegates.OnUpdated onUpdated;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    void Update()
    {
        onUpdated?.Invoke();
    }
}