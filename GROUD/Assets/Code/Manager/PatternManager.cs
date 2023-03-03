using System;
using System.Collections.Generic;
using System.Linq;
using Code.AI;
using NaughtyAttributes;
using Unity.VisualScripting;
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

    private float timer;
    private GameObject caster;


    private void Awake()
    {
        Instance = this;
    }

    public void StartPattern(Pattern p)
    {
        if (isTimelineActive) return;
        InitializeQueue(p.interactions);
        timer = 0;
        GameManager.onUpdated += TimelineEventListener;
        isTimelineActive = true;
    }

    private void InitializeQueue(List<InteractionKey> interactionKeys)
    {
        timelineRunnerKeys = new Queue<InteractionKey>();

        interactionKeys = interactionKeys.OrderBy(it => it.timeCode).ToList();

        foreach (var t in interactionKeys)
        {
            timelineRunnerKeys.Enqueue(t);
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

        if (timelineRunnerKeys.Count > 0) return;
        GameManager.onUpdated -= TimelineEventListener;
        PatternPoolManager.OnPatternEnd += EndOfPattern;
        isTimelineActive = false;
    }

    public void DrawInteractionOnScreen(InteractionKey dataKey)
    {
        GameObject interactionObj = null;
        InteractionComponent interactionComponent = null;
        
        caster = PatternPoolManager.Instance.GetCircleFromPool();
        
        
        int selectedWay = Helpers.GetRandomRange(0, GameManager.instance.spawnPoints.Length);
        Vector3 spawnPosition = GameManager.instance.spawnPoints[selectedWay].position;
        caster.transform.position = new Vector3(spawnPosition.x, 1, spawnPosition.z);
        
        caster.GetComponent<ExperienceOrb>().dataKey = dataKey;
        caster.transform.GetChild(0).GetComponent<TimerCircle>().ResetValues();
    }
}