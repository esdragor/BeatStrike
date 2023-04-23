using System;
using System.Collections.Generic;
using System.Linq;
using Code.Interface;
using UnityEngine;
using UnityEngine.Serialization;
using Utilities;

public class PatternManager : MonoBehaviour
{
    public static PatternManager Instance;
    public static Action OnPatternEnd;
    [FormerlySerializedAs("currentPattern")] public Pattern currentPatternSo;
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
            Debug.Log(t.time);
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
        Debug.Log("Draw interaction");

        caster = PatternPoolManager.Instance.GetCircleFromPool();
        caster.GetComponent<InteractionComponent>().SetData(dataKey);

        Vector3 spawnPosition = Vector3.zero;

        if (!isDebugMultiChannel)
        {
            switch (dataKey.interactionType)
            {
                case Enums.InteractionType.Tap:
                    spawnPosition = dataKey.row switch
                    {
                        0 => LevelManager.instance.leftSpawnPoint.position,
                        1 => LevelManager.instance.rightSpawnPoint.position,
                        _ => spawnPosition
                    };
                    break;
                
                case Enums.InteractionType.Swipe:
                    spawnPosition = LevelManager.instance.midSpawnPoint.position;
                    break;
            }

           
        }
        else
        {
            int randomIndex = dataKey.row;
            randomIndex = UnityEngine.Random.Range(0, LevelManager.instance.spinPoints.Length);
            spawnPosition = LevelManager.instance.spinPoints[randomIndex];

            spawnPosition.z = LevelManager.instance.DistanceToSpawnPointSpin;
        }


        caster.transform.position = spawnPosition;
    }
}