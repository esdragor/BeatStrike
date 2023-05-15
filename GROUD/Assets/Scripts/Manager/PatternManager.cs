using System;
using System.Collections.Generic;
using System.Linq;
using Code.Interface;
using UnityEngine;

public class PatternManager
{
    private Pattern currentPatternSo;
    private Queue<InteractionKey> timelineRunnerKeys;
    private bool isTimelineActive;
    private float timer;
    private bool isDebugMultiChannel;
    private GameObject interaction;

    public void StartPattern(Pattern p)
    {
        if (isTimelineActive) return;

        GameManager.instance.SetBPM(p.BPM);

        InitializeQueue(p.interactions);
        
        GameLoopManager.instance.tickCount = 0;
        
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
        
        if (timelineRunnerKeys.Count > 0)
        {
            if (Math.Abs(timelineRunnerKeys.Peek().frame - GameLoopManager.instance.tickCount) < 0.1f)
            {
                DrawInteractionOnScreen(timelineRunnerKeys.Dequeue());
            }
        }
        else
        {
            EndPattern();
        }
    }

    public void StopPattern()
    {
        isTimelineActive = false;
        GameManager.onUpdated -= TimelineEventListener;
    }
    
    
    public void EndPattern()
    {
        StopPattern();
        StartPattern(currentPatternSo);
    }

    public void DrawInteractionOnScreen(InteractionKey dataKey)
    {
        interaction = GameLoopManager.interactionPool.GetCircleFromPool();
        interaction.GetComponent<InteractionComponent>().SetData(dataKey);
        
        interaction.transform.position = GameLoopManager.instance.midSpawnPoint.position;;
    }
}