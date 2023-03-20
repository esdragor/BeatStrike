using System;
using UnityEngine;
using Utilities;

public class InputManager : MonoBehaviour
{
    public LevelManager levelManager;
    
    public InputListener leftListener;
    public InputListener rightListener;

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
        leftListener.onInputPressed += LookAtLeftDetector;
        rightListener.onInputPressed += LookAtRightDetector;
    }

    private void LookAtLeftDetector()
    {
        Debug.Log("Ca check left");
        InteractionComponent it = new TapInteraction();

        if (!levelManager.leftDetector.okDetector.IsInteractionEmpty())
        {
            it = levelManager.leftDetector.okDetector.interactions[0];
            levelManager.leftDetector.okDetector.interactions.Remove(it);
        }
        
        if (!levelManager.leftDetector.goodDetector.IsInteractionEmpty())
        {
            it = levelManager.leftDetector.goodDetector.interactions[0];
            levelManager.leftDetector.goodDetector.interactions.Remove(it);
        }
        
        if (!levelManager.leftDetector.perfectDetector.IsInteractionEmpty())
        { 
            it = levelManager.leftDetector.perfectDetector.interactions[0];
            levelManager.leftDetector.perfectDetector.interactions.Remove(it);
        }
        else
        {
            return;
        }

        
        switch (it.data.interactionType)
        {
            case Enums.InteractionType.Tap:
                it.ValidateInteraction();
                break;
            
            case Enums.InteractionType.Slide:
                Debug.Log("Slide logic doesn't exist already.");
                break;
        }
        
    }

    private void LookAtRightDetector()
    {
        Debug.Log("Ca check right");
        
        InteractionComponent it = new TapInteraction();

        if (!levelManager.rightDetector.okDetector.IsInteractionEmpty())
        {
            it = levelManager.rightDetector.okDetector.interactions[0];
            levelManager.rightDetector.okDetector.interactions.Remove(it);
        }
        
        if (!levelManager.rightDetector.goodDetector.IsInteractionEmpty())
        {
            it = levelManager.rightDetector.goodDetector.interactions[0];
            levelManager.rightDetector.goodDetector.interactions.Remove(it);
        }
        
        if (!levelManager.rightDetector.perfectDetector.IsInteractionEmpty())
        { 
            it = levelManager.rightDetector.perfectDetector.interactions[0];
            levelManager.rightDetector.perfectDetector.interactions.Remove(it);
        }
        else
        {
            return;
        }
        
        switch (it.data.interactionType)
        {
            case Enums.InteractionType.Tap:
                it.ValidateInteraction();
                break;
            
            case Enums.InteractionType.Slide:
                Debug.Log("Slide logic doesn't exist already.");
                break;
        }
    }

    void Disable()
    {
        
    }
}
