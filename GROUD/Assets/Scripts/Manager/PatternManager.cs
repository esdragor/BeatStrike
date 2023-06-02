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
    private int randomMultiplicator;

    private int attackPatternCount;
    private int defensePatternCount;

    public PatternManager(PatternByBPM[] _patternByBpms, int _percentageDef, int _randomMultiplicator)
    {
        patternByBpms = _patternByBpms;
        percentageDEF = _percentageDef;
        randomMultiplicator = _randomMultiplicator;
    }

    private bool lastWasDef;
    
    public bool StartPattern(bool isStart, float _remainingPulse = 0f)
    {
        bool isDef = false;
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
        int newPercentage = percentageDEF - (defensePatternCount * randomMultiplicator) + (attackPatternCount * randomMultiplicator);

        if (rnd < newPercentage)
        {
            pList = PatternBPM.Value.DEFPatterns;
            if (attackPatternCount > 0) attackPatternCount = 0;
            isDef = true;
            
            if (!lastWasDef)
            {
              PlayerManager.instance.vfxManager.AnnouncerPhaseVFX(isDef);
            }
            
            defensePatternCount++;
        }
        else
        {
            pList = PatternBPM.Value.ATKPatterns;
            if (defensePatternCount > 0) defensePatternCount = 0;

            if (lastWasDef)
            {
                PlayerManager.instance.vfxManager.AnnouncerPhaseVFX(isDef);
            }
            attackPatternCount++;
        }

        if (isStart)
        {
            PlayerManager.instance.vfxManager.AnnouncerPhaseVFX(isDef);
        }
        
        Pattern p = pList[UnityEngine.Random.Range(0, pList.Length)];
                
        UIManager.instance.DebugPattern(p.patternName);
        //GameManager.instance.SetBPM(p.BPM);

        InitializeQueue(p.interactions);

        GameLoopManager.instance.tickCount = _remainingPulse;
        
        GameManager.onUpdatedFrame = TimelineEventListener;

        isTimelineActive = true;

        lastWasDef = isDef;
        return (isDef);
    }

    private void InitializeQueue(List<InteractionKey> interactionKeys)
    {
        timelineRunnerKeys = new Queue<InteractionKey>();

        interactionKeys = interactionKeys.OrderBy(it => it.frame).ToList();

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
        if (timelineRunnerKeys == null) return;
        timelineRunnerKeys.Clear();
        GameLoopManager.interactionPool.DisableAllInteractions();
        
        isTimelineActive = false;
        GameManager.onUpdated = null;
        
        //EndPattern(false);
    }

    public async void EndPattern(bool EndPattern = true)
    {
        float timer = 3f;
        
        isTimelineActive = false;
        GameManager.onUpdated = null;
        timelineRunnerKeys.Clear();
        while (timer > 0)
        {
            timer -= Time.deltaTime * GameManager.instance.GetTickRate();
            await Task.Yield();
        }
        if (EndPattern && !GameLoopManager.instance.IsMoving)
            GameLoopManager.instance.PrintComboRoad(StartPattern(false, -timer));
        
    }

    public void DrawInteractionOnScreen(InteractionKey dataKey)
    {
        GameObject interaction = GameLoopManager.interactionPool.GetCircleFromPool();
        interaction.GetComponent<InteractionComponent>().SetData(dataKey);

        interaction.transform.position = GameLoopManager.instance.midSpawnPoint.position;
        ;
    }
}