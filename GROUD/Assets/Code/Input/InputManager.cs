using UnityEngine;
using Utilities;

public class InputManager : MonoBehaviour
{
    public LevelManager levelManager;
    
    public InputListener inputListener;

    private void Awake()
    {
        Enable();
    }

    void Enable()
    {
        inputListener.onInputPressed += TapCheckDetector;
        inputListener.onSwipeDetected += SwipeCheckDetector;
    }

    private void TapCheckDetector(InputListener.TouchSide touchSide)
    {
        if (!levelManager.detector) return;

        InteractionKey.InteractionColor color = InteractionKey.InteractionColor.Blue;
        
        switch (touchSide)
        {
            case InputListener.TouchSide.LEFT:
                color = InteractionKey.InteractionColor.Blue;
                break;
            
            case InputListener.TouchSide.RIGHT:
                color = InteractionKey.InteractionColor.Red;
                break;
        }
        
        InteractionComponent it = levelManager.detector.PeekInteraction();
        if (!it || it.data.interactionType != Enums.InteractionType.Tap) return;
        if(it.data.interactionColor == color) it.ValidateInteraction();
    }

    private void SwipeCheckDetector(InputListener.SwipeDirection direction)
    {
        Debug.Log("Swipe");
        
        if (!levelManager.detector) return;

        InteractionComponent it = levelManager.detector.PeekInteraction();
        
        if (!it || it.data.interactionType != Enums.InteractionType.Swipe) return;

        Debug.Log($"It's {it.data.swipeDirection} ans you do {direction}");
        if (it.data.swipeDirection == direction)
        {
            it.ValidateInteraction();
            Debug.Log("OK !");
        }
    }
}