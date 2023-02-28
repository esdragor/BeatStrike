using System;
using System.Collections.Generic;
using System.Linq;
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
    public Queue<InteractionKey> timelineRunnerKeys;
    public List<InteractionKey> interactionKeys;
    public float timer;
    
    private void Awake()
    {
        Instance = this;

        timelineRunnerKeys = new Queue<InteractionKey>();
    }

    private void Start()
    {
        interactionKeys = interactionKeys.OrderBy(it => it.timeCode).ToList();
        
        for (int i = 0; i < interactionKeys.Count; i++)
        {
            timelineRunnerKeys.Enqueue(interactionKeys[i]);
        }
        
        Logs.Log("First of Queue", timelineRunnerKeys.Peek().name, LogType.Log, Logs.LogColor.Green);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        
        TimelineEventListener();
    }

    private void TimelineEventListener()
    {
        if(timelineRunnerKeys.Count <= 0) return;

        if (Math.Abs(timelineRunnerKeys.Peek().timeCode - timer) < 0.1f)
        {
            DrawInteractionOnScreen(timelineRunnerKeys.Dequeue());
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

    void Debug()
    {
    }
}
