using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Code.Interface;
using UnityEngine;

public class PatternManager
{
    public bool isTimelineActive;
    private Queue<InteractionKey> timelineRunnerKeys;
    private float timer;
    private bool isDebugMultiChannel;
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
        {
            pList = DEFPatterns;
            UIManager.instance.announcer.Announce("DEFENSE", Color.white);
        }
        else
        {
            pList = ATKPatterns;
            UIManager.instance.announcer.Announce("ATTACK", Color.white);
        }

        Pattern p = pList[UnityEngine.Random.Range(0, pList.Count)];

        //GameManager.instance.SetBPM(p.BPM);

        InitializeQueue(p.interactions);

        GameLoopManager.instance.tickCount = 0;

        timer = 0;

        GameManager.onUpdatedFrame = TimelineEventListener;

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
        if (!isTimelineActive) return;
        if (timelineRunnerKeys.Count > 0)
        {
            float abs = timelineRunnerKeys.Peek().frame - GameLoopManager.instance.tickCount;
            if (abs < 0f) abs = 0f;
            if (abs < 0.1f)
            {
                DrawInteractionOnScreen(timelineRunnerKeys.Dequeue());
            }
        }
        else if (!GameLoopManager.instance.IsMoving)
        {
            EndPattern();
        }
    }

    public void StopPattern()
    {
        GameLoopManager.interactionPool.DisableAllInteractions();
        EndPattern(false);
    }


    public async void EndPattern(bool EndPattern = true)
    {
        isTimelineActive = false;
        GameManager.onUpdated = null;
        await Task.Delay(1000);
        if (EndPattern && !GameLoopManager.instance.IsMoving)
            GameLoopManager.instance.printDEFRoad(StartPattern());
    }

    public void DrawInteractionOnScreen(InteractionKey dataKey)
    {
        GameObject interaction = GameLoopManager.interactionPool.GetCircleFromPool();
        interaction.GetComponent<InteractionComponent>().SetData(dataKey);

        interaction.transform.position = GameLoopManager.instance.midSpawnPoint.position;
        ;
    }
}