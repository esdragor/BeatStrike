using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

public class LevelRoadManager : MonoBehaviour
{
    [Range(1, 100)] public int stepCount = 1;
    [Range(1, 50)] public int subStepByStep = 10; 
    [Range(0.1f, 5f)]public float stepDistance = 1f; //1 meter
    private float distanceSplit => stepDistance / subStepByStep;
    public int subStepCount;
    int SubStepCount() => subStepCount = subStepByStep * stepCount;

    public List<RoadStep> steps;

    [Button("Init Road Steps")] void InitRoadSteps()
    {
        steps = new List<RoadStep>();
        for (int i = 0; i < stepCount; i++)
        {
            steps.Add(new RoadStep());
        }
    }

    void SetRoadStepPosition()
    {
        if (steps.Count < stepCount)
        {
            int countToAdd = stepCount - steps.Count;
            for (int i = 0; i < countToAdd; i++)
            {
                steps.Add(new RoadStep());
            }
        }
        else
        {
            int countToRemove = steps.Count - stepCount;
            for (int i = 0; i < countToRemove; i++)
            {
                steps.Remove(steps[^1]);

            }
        }
        
        for (int i = 0; i < steps.Count; i++)
        {
            float padding = i * stepDistance;
            steps[i].position = new Vector3(transform.position.x, transform.position.y, transform.position.z + padding);
            
            steps[i].subStepPosition = new Vector3[subStepByStep];
            for (int j = 0; j < subStepByStep; j++)
            {
                float subPadding = j * distanceSplit;
                Vector3 subPosition = new Vector3(steps[i].position.x, steps[i].position.y, steps[i].position.z + subPadding);
                steps[i].subStepPosition[j] = subPosition;
            }
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        SetRoadStepPosition();
        for (int i = 0; i < steps.Count; i++)
        {
            switch (steps[i].stepAction)
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
            
            Gizmos.DrawRay(steps[i].position, Vector3.up * 2);

            if (i < stepCount - 1)
            {
                for (int j = 0; j < steps[i].subStepPosition.Length; j++)
                {
                    if (steps[i].position != steps[i].subStepPosition[j])
                    {
                        Gizmos.color = Color.grey;
                        Gizmos.DrawRay(steps[i].subStepPosition[j], Vector3.up * 0.5f);
                    }
                }
            }
        }
    }

    [Serializable] public class RoadStep
    {
        public StepAction stepAction;
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
