 using UnityEngine;

 public class GameState
 {

     public GameState(LevelState levelState, TimeState timeState, EngineState engineState)
     {
         this.levelState = levelState;
         this.timeState = timeState;
         this.engineState = engineState;
     }
     
     public enum EngineState
     {
         Menu,
         Game
     }

     public enum TimeState
     {
         Play,
         Pause
     }
     
     public enum LevelState
     {
         Exploration,
         Boss
     }

     private LevelState levelState;
     private TimeState timeState;
     private EngineState engineState;

     public void SwitchLevelState(LevelState state) => levelState = state;
     public void SwitchTimeState(TimeState state) => timeState = state;
     public void SwitchEngineState(EngineState state) => engineState = state;

     public bool IsTimePlay() => timeState == TimeState.Play;
     public bool IsTimePause() => timeState == TimeState.Pause;
     
     public bool IsLevelExploration() => levelState == LevelState.Exploration;
     public bool IsLevelBoss() => levelState == LevelState.Boss;

     public bool IsEngineMenu() => engineState == EngineState.Menu;
     public bool IsEngineGame() => engineState == EngineState.Game;

 }
