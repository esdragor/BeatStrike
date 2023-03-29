using UnityEngine;
using Utilities;

public class InputManager : MonoBehaviour
{
    public LevelManager levelManager;
    public InputListener inputListener;
    private InteractionComponent selectedSwipeIt;

    void OnEnable()
    {
        inputListener.onInputPressed += TapBehaviour;
        inputListener.onSwipeDetected += SwipeBehaviour;
    }
    private void OnDisable()
    {
        inputListener.onInputPressed -= TapBehaviour;
        inputListener.onSwipeDetected -= SwipeBehaviour;
    }

    private void SwipeBehaviour(InputListener.SwipeDirection dir)
    {
        
        if ( levelManager.detector.currentIt != null &&  levelManager.detector.currentIt.data.swipeDirection == dir &&  levelManager.detector.currentIt.data.interactionType == Enums.InteractionType.Swipe)
        {
            levelManager.detector.currentIt.ValidateInteraction();
        }
    }

    private void TapBehaviour(InputListener.TouchSide touchSide)
    {
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
        
        if (levelManager.detector.currentIt != null &&  levelManager.detector.currentIt.data.interactionColor == color &&  levelManager.detector.currentIt.data.interactionType == Enums.InteractionType.Tap)
        {
            levelManager.detector.currentIt.ValidateInteraction();
        }
    }
    
}