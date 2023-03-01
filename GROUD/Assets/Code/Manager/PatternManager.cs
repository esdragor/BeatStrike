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
    public static event Action OnPatternEnd;
    public InteractionKey currentInteraction;
    private Queue<InteractionKey> timelineRunnerKeys;
    public bool isTimelineActive;

    [Expandable] public Pattern testPattern;

    private float timer;
    private GameObject caster;
    private Vector3 target;

    private void Awake()
    {
        Instance = this;
    }

    public void StartPattern(Pattern p, GameObject obj, Vector3 _target)
    {
        caster = obj;
        target = _target;
        if (!isTimelineActive)
        {
            InitializeQueue(p.interactions);
            timer = 0;
            GameManager.onUpdated += TimelineEventListener;
            isTimelineActive = true;
        }

    }

    private void InitializeQueue(List<InteractionKey> interactionKeys)
    {
        timelineRunnerKeys = new Queue<InteractionKey>();

        interactionKeys = interactionKeys.OrderBy(it => it.timeCode).ToList();

        for (int i = 0; i < interactionKeys.Count; i++)
        {
            timelineRunnerKeys.Enqueue(interactionKeys[i]);
        }
    }
    
    private void EndOfPattern()
    {
        OnPatternEnd?.Invoke();
        PatternPoolManager.OnPatternEnd -= EndOfPattern;
        isTimelineActive = false;
    }

    private void TimelineEventListener()
    {
        timer += Time.deltaTime;

        if (Math.Abs(timelineRunnerKeys.Peek().timeCode - timer) < 0.1f)
        {
            DrawInteractionOnScreen(timelineRunnerKeys.Dequeue());
        }

        if (timelineRunnerKeys.Count <= 0)
        {
            GameManager.onUpdated -= TimelineEventListener;
           PatternPoolManager.OnPatternEnd += EndOfPattern;
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
                interactionObj = caster.transform.GetChild(0).gameObject;
                interactionComponent = interactionObj.GetComponent<InteractionComponent>();
                TapInteraction tapIn = (TapInteraction)interactionComponent;
                interactionComponent.speed = caster.transform.position.z;
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