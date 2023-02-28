using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using Utilities;

public enum SwipeDirection
{
    Up,
    Down,
    Left,
    Right
}

public class PatternManager : MonoBehaviour
{
    public static PatternManager Instance;
    public InteractionKey currentInteraction;
    private Queue<InteractionKey> timelineRunnerKeys;
    
    [Expandable] public Pattern testPattern;
    
    private float timer;
    public bool isTimelineActive;
    
    private void Awake()
    {
        Instance = this;
    }

    public void StartPattern(Pattern p)
    {
        if (!isTimelineActive)
        {
            InitializeQueue(p.interactions);
            timer = 0;
            GameManager.onUpdated += TimelineEventListener;
            isTimelineActive = true;
        }
        else
        {
            Logs.Log("Pattern Manager", "Timeline is already active, you can't load a pattern", LogType.Error, Logs.LogColor.Red, Logs.LogColor.None);
        }
    }
    
    private void InitializeQueue(List<InteractionKey> interactionKeys)
    {
        if (timelineRunnerKeys != null)
        {
            timelineRunnerKeys.Clear();
        }
        else
        {
            timelineRunnerKeys = new Queue<InteractionKey>();
        }
        
        interactionKeys = interactionKeys.OrderBy(it => it.timeCode).ToList();
        
        for (int i = 0; i < interactionKeys.Count; i++)
        {
            timelineRunnerKeys.Enqueue(interactionKeys[i]);
        }
    }
    
    private void TimelineEventListener()
    {
        timer += Time.deltaTime;

        if(timelineRunnerKeys.Count <= 0) return;

        if (Math.Abs(timelineRunnerKeys.Peek().timeCode - timer) < 0.1f)
        {
            DrawInteractionOnScreen(timelineRunnerKeys.Dequeue());
        }

        if (timelineRunnerKeys.Count <= 0)
        {
            GameManager.onUpdated -= TimelineEventListener;
            isTimelineActive = false;
        }

    }

    public void DrawInteractionOnScreen(InteractionKey dataKey)
    {
        GameObject interactionObj = null;
        InteractionComponent interactionComponent = null;
        
        switch (dataKey.interactionType)
        {
            case Enums.InteractionType.Tap:
                interactionObj = PatternPoolManager.Instance.GetCircleFromPool();
                interactionComponent = interactionObj.GetComponent<InteractionComponent>();
                TapInteraction tapIn = (TapInteraction)interactionComponent;
                tapIn.SetData(dataKey);
                break;
            
            case Enums.InteractionType.Hold:
                break;
            
            case Enums.InteractionType.Slide:
                break;
            
            case Enums.InteractionType.Spam:
                break;
        }

        if (interactionComponent != null) interactionComponent.StartInteraction();
    }
}
