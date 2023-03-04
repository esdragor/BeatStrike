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
    public Pattern currentPattern;
    public InteractionKey currentInteraction;
    private Queue<InteractionKey> timelineRunnerKeys;
    public bool isTimelineActive;

    public float timer;
    private GameObject caster;


    private void Awake()
    {
        Instance = this;
    }

    public void StartPattern(Pattern p)
    {
        if (isTimelineActive) return;
        InitializeQueue(p.interactions);
        currentPattern = p;
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
        Debug.Log("End of pattern");
        OnPatternEnd?.Invoke();
        PatternPoolManager.OnPatternEnd -= EndOfPattern;
        isTimelineActive = false;
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
        
        if (timer > currentPattern.maxTime)
        {
            PatternPoolManager.OnPatternEnd += EndOfPattern;
            PatternPoolManager.Instance.InvokePatternEnd();
            isTimelineActive = false;
            GameManager.onUpdated -= TimelineEventListener;
        }
        
    }

    public void DrawInteractionOnScreen(InteractionKey dataKey)
    {
        GameObject interactionObj = null;
        InteractionComponent interactionComponent = null;
        
        caster = PatternPoolManager.Instance.GetCircleFromPool();


        int selectedWay = dataKey.row;
        Vector3 spawnPosition = GameManager.instance.spawnPoints[selectedWay].position;
        caster.transform.position = new Vector3(spawnPosition.x, 1, spawnPosition.z);
        
        caster.GetComponent<ExperienceOrb>().dataKey = dataKey;
        caster.transform.GetChild(0).GetComponent<TimerCircle>().ResetValues();
    }
}