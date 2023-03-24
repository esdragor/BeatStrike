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
    public static Action OnPatternEnd;
    public Pattern currentPattern;
    public InteractionKey currentInteraction;
    private Queue<InteractionKey> timelineRunnerKeys;
    public bool isTimelineActive;

    public float timer;

    public bool isDebugMultiChannel = false;
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
            ForceEnd();
        }
    }


    public void ForceEnd()
    {
        isTimelineActive = false;
        OnPatternEnd?.Invoke();
        GameManager.onUpdated -= TimelineEventListener;
    }

    public void DrawInteractionOnScreen(InteractionKey dataKey)
    {
        caster = PatternPoolManager.Instance.GetCircleFromPool();
        caster.GetComponent<InteractionComponent>().SetData(dataKey);

        Vector3 spawnPosition = Vector3.zero;

        if (!isDebugMultiChannel)
            switch (dataKey.row)
            {
                case 0:
                    spawnPosition = LevelManager.instance.leftSpawnPoint.position;
                    break;

                case 1:
                    spawnPosition = LevelManager.instance.rightSpawnPoint.position;
                    break;
            }
        else
        {
            int randomIndex = dataKey.row;
            randomIndex = UnityEngine.Random.Range(0, 6);
            switch (randomIndex)
            {
                case 0:
                    spawnPosition = LevelManager.instance.spinPoints[0].position;
                    break;

                case 1:
                    spawnPosition = LevelManager.instance.spinPoints[1].position;
                    break;

                case 2:
                    spawnPosition = LevelManager.instance.spinPoints[2].position;
                    break;

                case 3:
                    spawnPosition = LevelManager.instance.spinPoints[3].position;
                    break;

                case 4:
                    spawnPosition = LevelManager.instance.spinPoints[4].position;
                    break;

                case 5:
                    spawnPosition = LevelManager.instance.spinPoints[5].position;
                    break;
            }

            spawnPosition.z = 30;
        }

        caster.transform.position = new Vector3(spawnPosition.x, 1, spawnPosition.z);
    }
}