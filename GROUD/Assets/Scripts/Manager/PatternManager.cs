using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Code.Interface;
using UnityEngine;

public class PatternManager
{
    private Queue<InteractionKey> timelineRunnerKeys;
    private bool isTimelineActive;
    private float timer;
    private bool isDebugMultiChannel;
    private GameObject interaction;
    private List<Pattern> ATKPatterns;
    private List<Pattern> DEFPatterns;
    private int percentageDEF;

    public PatternManager(List<Pattern> _ATKPatterns, List<Pattern> _DEFPatterns, int _percentageDEF)
    {
        ATKPatterns = _ATKPatterns;
        DEFPatterns = _DEFPatterns;
        percentageDEF = _percentageDEF;
    }

    public bool StartPattern()
    {
        if (isTimelineActive) return false;

        List<Pattern> pList = null;

        int rnd = UnityEngine.Random.Range(0, 100);
        if (rnd < percentageDEF)
            pList = DEFPatterns;
        else
            pList = ATKPatterns;

        Pattern p = pList[UnityEngine.Random.Range(0, pList.Count)];

        GameManager.instance.SetBPM(p.BPM);

        InitializeQueue(p.interactions);

        GameLoopManager.instance.tickCount = 0;

        timer = 0;

        GameManager.onUpdated += TimelineEventListener;

        isTimelineActive = true;

        return (rnd < percentageDEF);
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
            float abs = timelineRunnerKeys.Peek().frame - GameLoopManager.instance.tickCount;
            if (abs < 0f) abs = 0f;
            if (abs < 0.1f)
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


    public async void EndPattern()
    {
        StopPattern();
        await Task.Delay(1000);
        GameLoopManager.instance.printDEFRoad(StartPattern());
    }

    public void DrawInteractionOnScreen(InteractionKey dataKey)
    {
        interaction = GameLoopManager.interactionPool.GetCircleFromPool();
        interaction.GetComponent<InteractionComponent>().SetData(dataKey);

        interaction.transform.position = GameLoopManager.instance.midSpawnPoint.position;
        ;
    }
}