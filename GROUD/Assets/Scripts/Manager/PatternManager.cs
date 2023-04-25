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
    private GameObject interaction;

    public void StartPattern(Pattern p)
    {
        if (isTimelineActive) return;

        GameManager.instance.SetBPM(p.BPM);

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
       // timer += Time.deltaTime;
        
        if (timelineRunnerKeys.Count > 0)
        {
            if (Math.Abs(timelineRunnerKeys.Peek().frame - GameLoopManager.instance.tickCount) < 0.1f)
            {
                Debug.Log($" Frame : {timelineRunnerKeys.Peek().frame} | {GameLoopManager.instance.tickCount}");
                DrawInteractionOnScreen(timelineRunnerKeys.Dequeue());
            }
        }
        else
        {
            EndPattern();
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
        interaction = GameLoopManager.interactionPool.GetInteractionFromPool();
        interaction.GetComponent<InteractionComponent>().SetData(dataKey);

        Vector3 spawnPosition = Vector3.zero;

        switch (dataKey.interactionType)
        {
            case Enums.InteractionType.Attack:
                spawnPosition = dataKey.row switch
                {
                    0 => GameLoopManager.instance.leftSpawnPoint.position,
                    1 => GameLoopManager.instance.rightSpawnPoint.position,
                    _ => spawnPosition
                };
                break;
                
            default:
                spawnPosition = GameLoopManager.instance.midSpawnPoint.position;
                break;
        }

        interaction.transform.position = spawnPosition;
    }
}