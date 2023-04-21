using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class LevelRoadManager : MonoBehaviour
{
    [Range(1, 500)] public int stepCount = 1;
    [Range(1, 50)] public int subStepByStep = 10; 
    [Range(0.1f, 5f)]public float stepDistance = 1f;
    private float distanceSplit => stepDistance / subStepByStep;
    public List<RoadStep> majorSteps;
    private List<RoadStep> steps;
    private int currentIndex;

    [Button("Init Road Steps")] void InitRoadSteps()
    {
        majorSteps = new List<RoadStep>();
        for (int i = 0; i < stepCount; i++)
        {
            majorSteps.Add(new RoadStep());
        }
    }

    private void Awake()
    {
        SetRoadStepPosition();
    }

    public void Restart()
    {
        currentIndex = 0;
    }
    
    public void CheckStepsToTarget(int stepLenght)
    {
        if(GameManager.instance.gameState.IsLevelCombat()) return;
        int targetIndex = currentIndex + stepLenght;
        
        if (targetIndex > steps.Count)
        {
            targetIndex = steps.Count;
        }
        
        for (int i = currentIndex; i < targetIndex; i++)
        {
            switch (steps[i].stepAction)
            {
                case RoadStep.StepAction.NONE:
                    break;
                
                case RoadStep.StepAction.ENNEMY:
                    if (steps[i].complete) break;
                        
                    if (!GameManager.instance.gameState.IsLevelCombat())
                    {                  
                        LevelManager.instance.SetCombatMode();
                    }
                    
                    steps[i].complete = true;
                    
                    return;
                
                case RoadStep.StepAction.END:
                    PlayerManager.instance.MovePlayerTo(steps[i].position, RoadStep.StepAction.END);
                    steps[i].complete = true;
                    return;
            }
            
            
            if (i == targetIndex -1)
            {
                PlayerManager.instance.MovePlayerTo(steps[i].position);
                steps[i].complete = true;
                currentIndex = targetIndex;
            }
        }
    }

    void SetRoadStepPosition()
    {
        steps = new List<RoadStep>();
        
        if (majorSteps.Count < stepCount)
        {
            int countToAdd = stepCount - majorSteps.Count;
            for (int i = 0; i < countToAdd; i++)
            {
                majorSteps.Add(new RoadStep());
            }
        }
        else
        {
            int countToRemove = majorSteps.Count - stepCount;
            for (int i = 0; i < countToRemove; i++)
            {
                majorSteps.Remove(majorSteps[^1]);
            }
        }
        
        for (int i = 0; i < majorSteps.Count; i++)
        {
            float padding = i * stepDistance;
            majorSteps[i].position = new Vector3(transform.position.x, transform.position.y, transform.position.z + padding);
            majorSteps[i].subStepPosition = new Vector3[subStepByStep];
            
            for (int j = 0; j < subStepByStep; j++)
            {
                float subPadding = j * distanceSplit;
                Vector3 subPosition = new Vector3(majorSteps[i].position.x, majorSteps[i].position.y, majorSteps[i].position.z + subPadding);
                majorSteps[i].subStepPosition[j] = subPosition;
                
                steps.Add(majorSteps[i]);
                steps.Add(new RoadStep{position = majorSteps[i].subStepPosition[j]});
            }
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        SetRoadStepPosition();
        
        for (int i = 0; i < majorSteps.Count; i++)
        {
            switch (majorSteps[i].stepAction)
            {
                case RoadStep.StepAction.NONE:
                    Gizmos.color = Color.black;
                    break;
                
                case RoadStep.StepAction.ENNEMY:
                    Gizmos.color = Color.green;

                    break;
                
                case RoadStep.StepAction.END:
                    Gizmos.color = Color.red;
                    break;
            }
            
            Gizmos.DrawRay(majorSteps[i].position, Vector3.up * 2);

            if (i < stepCount - 1)
            {
                for (int j = 0; j < majorSteps[i].subStepPosition.Length; j++)
                {
                    if (majorSteps[i].position != majorSteps[i].subStepPosition[j])
                    {
                        Gizmos.color = Color.grey;
                        Gizmos.DrawRay(majorSteps[i].subStepPosition[j], Vector3.up * 0.5f);
                    }
                }
            }
        }
    }

    [Serializable] public class RoadStep
    {
        public StepAction stepAction;
        public int index;
        public bool complete;
        public Vector3 position;
        public Vector3[] subStepPosition;

        public enum StepAction
        {
            NONE,
            ENNEMY,
            END
        }
    }
}
