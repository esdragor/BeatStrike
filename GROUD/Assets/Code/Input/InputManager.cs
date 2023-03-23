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

        InteractionComponent it = levelManager.detector.PeekInteraction();
        if (!it) return;
        Debug.Log(it.data.interactionColor);
        if(it.data.interactionColor == color) it.ValidateInteraction();
    }

    void Disable()
    {
        blueInputListener.onInputPressed -= CheckDetector;
        redInputListener.onInputPressed -= CheckDetector;
    }
}
