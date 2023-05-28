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
    private bool isDebugMultiChannel;
    private PatternByBPM[] patternByBpms;
    private int percentageDEF;

    public PatternManager(PatternByBPM[] _patternByBpms, int _percentageDef)
    {
        patternByBpms = _patternByBpms;
        percentageDEF = _percentageDef;
    }

    public bool StartPattern(float _remainingPulse = 0f)
    {
        if (isTimelineActive) return false;

        Pattern[] pList = null;

        float BPM = GameManager.instance.Bpm;
        
        PatternByBPM? PatternBPM = null;

        foreach (var pat in patternByBpms)
        {
            if (pat.bpm == BPM)
            {
                PatternBPM = pat;
                break;
            }
        }
        
        if (PatternBPM == null)
        {
            throw new Exception("No pattern found for this BPM");
        }

        int rnd = UnityEngine.Random.Range(0, 100);
        if (rnd < percentageDEF)
        {
            pList = PatternBPM.Value.DEFPatterns;
            UIManager.instance.announcer.Announce("DEFENSE", Color.white);
        }
        else
        {
            pList = PatternBPM.Value.ATKPatterns;
            UIManager.instance.announcer.Announce("ATTACK", Color.white);
        }

        Pattern p = pList[UnityEngine.Random.Range(0, pList.Length)];

        //GameManager.instance.SetBPM(p.BPM);

        InitializeQueue(p.interactions);

        GameLoopManager.instance.tickCount = _remainingPulse;
        
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
        float timer = 1f;
        
        isTimelineActive = false;
        GameManager.onUpdated = null;
        while (timer > 0)
        {
            timer -= (1f /60f) * GameManager.instance.Bpm;
            await Task.Yield();
        }
        if (EndPattern && !GameLoopManager.instance.IsMoving)
            GameLoopManager.instance.printDEFRoad(StartPattern(-timer));
    }

    public void DrawInteractionOnScreen(InteractionKey dataKey)
    {
        GameObject interaction = GameLoopManager.interactionPool.GetCircleFromPool();
        interaction.GetComponent<InteractionComponent>().SetData(dataKey);

        interaction.transform.position = GameLoopManager.instance.midSpawnPoint.position;
        ;
    }
}