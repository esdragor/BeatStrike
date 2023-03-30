 using System;
 using Utilities;

 public class GameState
 {
     public Action<Enums.EngineState> OnEngineStateChanged;
     public Action<Enums.TimeState> OnTimeStateChanged;
     public Action<Enums.LevelState> OnLevelStateChanged;
     
     public GameState(Enums.LevelState levelState, Enums.TimeState timeState, Enums.EngineState engineState)
     {
         this.levelState = levelState;
         this.timeState = timeState;
         this.engineState = engineState;
     }

     void CallAllGameState()
     {
         OnLevelStateChanged?.Invoke(levelState);
         OnTimeStateChanged?.Invoke(timeState);
         OnEngineStateChanged?.Invoke(engineState);
     }
     
     private Enums.LevelState levelState;
     private Enums.TimeState timeState;
     private Enums.EngineState engineState;

     public void SwitchLevelState(Enums.LevelState state)
     {
         levelState = state;
         OnLevelStateChanged?.Invoke(levelState);
     }
     public void SwitchTimeState(Enums.TimeState state)
     {
         timeState = state;
         OnTimeStateChanged?.Invoke(timeState);
     }

     public void SwitchEngineState(Enums.EngineState state)
     {
         engineState = state;
         OnEngineStateChanged?.Invoke(engineState);
     }

     public bool IsTimePlay() => timeState == Enums.TimeState.Play;
     public bool IsTimePause() => timeState == Enums.TimeState.Pause;
     
     public bool IsLevelExploration() => levelState == Enums.LevelState.Exploration;
     public bool IsLevelBoss() => levelState == Enums.LevelState.Boss;

     public bool IsEngineMenu() => engineState == Enums.EngineState.Menu;
     public bool IsEngineGame() => engineState == Enums.EngineState.Game;

 }
