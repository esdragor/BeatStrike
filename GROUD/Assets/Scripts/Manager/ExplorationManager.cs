public class ExplorationManager
{
        public void InitExploration(Pattern p)
        {
                LevelManager.patternManager.StartPattern(p);
        }

        public void CorridorEndReached()
        {
                LevelManager.instance.CheckForNextPattern();
        }
}