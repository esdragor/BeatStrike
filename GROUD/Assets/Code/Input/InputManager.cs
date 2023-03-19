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

        if(levelManager.leftDetector.IsInteractionEmpty()) return;
        InteractionComponent it = levelManager.leftDetector.interactions[0];
        
        switch (it.data.interactionType)
        {
            case Enums.InteractionType.Tap:
                it.ValidateInteraction();
                levelManager.leftDetector.interactions.Remove(it);
                break;
            
            case Enums.InteractionType.Slide:
                Debug.Log("Slide logic doesn't exist already.");
                break;
        }
        
    }

    private void LookAtRightDetector()
    {
        Debug.Log("Ca check right");

        if(levelManager.rightDetector.IsInteractionEmpty()) return;

        InteractionComponent it = levelManager.rightDetector.interactions[0];
        
        switch (it.data.interactionType)
        {
            case Enums.InteractionType.Tap:
                it.ValidateInteraction();
                levelManager.rightDetector.interactions.Remove(it);
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
