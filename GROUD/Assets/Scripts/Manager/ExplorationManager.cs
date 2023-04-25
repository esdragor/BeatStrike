public class ExplorationManager
{
        public void InitExploration(Pattern p)
        {
                GameLoopManager.patternManager.StartPattern(p);
        }

        public void CorridorEndReached()
        {
                GameLoopManager.instance.CheckForNextPattern();
        }
}