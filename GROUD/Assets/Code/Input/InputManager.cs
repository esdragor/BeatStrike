using System;
using UnityEngine;
using UnityEngine.Serialization;
using Utilities;

public class InputManager : MonoBehaviour
{
    public LevelManager levelManager;
    
    [FormerlySerializedAs("leftListener")] public InputListener blueInputListener;
    [FormerlySerializedAs("rightListener")] public InputListener redInputListener;

    private void Awake()
    {
        Enable();
    }

    private void OnDisable()
    {
        Disable();
    }

    void Enable()
    {
        blueInputListener.onInputPressed += CheckDetector;
        redInputListener.onInputPressed += CheckDetector;
    }

    private void CheckDetector(InteractionKey.InteractionColor color)
    {
        if (!levelManager.detector) return;
        InputListener selectedListener = null;
        
        switch (color)
        {
            case InteractionKey.InteractionColor.Blue:
                selectedListener = blueInputListener;
                break;
            
            case InteractionKey.InteractionColor.Red:
                selectedListener = redInputListener;
                break;
        }
        
        if(selectedListener == null) return;

        InteractionComponent it;
        
        if (!levelManager.detector.okDetector.IsInteractionEmpty())
        {
            it = levelManager.detector.okDetector.interactions[0];
            levelManager.detector.okDetector.interactions.Remove(it);
        }
        else
        {
            if (!levelManager.detector.goodDetector.IsInteractionEmpty())
            {
                it = levelManager.detector.goodDetector.interactions[0];
                levelManager.detector.goodDetector.interactions.Remove(it);
            }
            else
            {
                if (!levelManager.detector.perfectDetector.IsInteractionEmpty())
                { 
                    it = levelManager.detector.perfectDetector.interactions[0];
                    levelManager.detector.perfectDetector.interactions.Remove(it);
                }
                else
                {
                    return;
                }
            }
        }
        
        if (selectedListener.listenerColor == it.data.interactionColor)
        {
            it.ValidateInteraction();
        }
        else
        {
            it.HurtPlayer();
        }
        
    }

    void Disable()
    {
        blueInputListener.onInputPressed -= CheckDetector;
        redInputListener.onInputPressed -= CheckDetector;
    }
}
