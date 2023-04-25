using System;
using System.Collections.Generic;
using System.Linq;
using Code.Interface;
using UnityEngine;
using Utilities;

public class PatternManager
{
    private Pattern currentPatternSo;
    private Queue<InteractionKey> timelineRunnerKeys;
    private bool isTimelineActive;
    private float timer;
    private bool isDebugMultiChannel;
    private GameObject caster;

    public void StartPattern(Pattern p)
    {
        if (isTimelineActive) return;

        InitializeQueue(p.interactions);

        currentPatternSo = p;
        timer = 0;

        GameManager.onUpdated += TimelineEventListener;

        isTimelineActive = true;
    }

    private void InitializeQueue(List<InteractionKey> interactionKeys)
    {
        timelineRunnerKeys = new Queue<InteractionKey>();

        interactionKeys = interactionKeys.OrderBy(it => it.time).ToList();
        
        foreach (var t in interactionKeys)
        {
            timelineRunnerKeys.Enqueue(t);
        }
    }
    
    private void TimelineEventListener()
    {
        timer += Time.deltaTime;

        if (timelineRunnerKeys.Count > 0)
        {

            if (Math.Abs(timelineRunnerKeys.Peek().time - timer) < 0.1f)
            {
                DrawInteractionOnScreen(timelineRunnerKeys.Dequeue());
            }
        }

        if (timer > currentPatternSo.maxTime)
        {
            EndPattern();
        }
    }


    public void EndPattern()
    {
        isTimelineActive = false;
        GameManager.onUpdated -= TimelineEventListener;
        
        if (GameManager.gameState.IsLevelExploration())
        {
            GameLoopManager.explorationManager.CorridorEndReached();
        }
    }

    public void DrawInteractionOnScreen(InteractionKey dataKey)
    {
        caster = GameLoopManager.interactionPool.GetCircleFromPool();
        caster.GetComponent<InteractionComponent>().SetData(dataKey);
        
        caster.transform.position = GameLoopManager.instance.midSpawnPoint.position;;
    }
}