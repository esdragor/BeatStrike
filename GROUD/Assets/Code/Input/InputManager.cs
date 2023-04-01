using Code.Interface;
using UnityEngine;
using Utilities;

public class InputManager : MonoBehaviour
{
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

    private void Start()
    {
        if(GameManager.instance.gameState.IsEngineMenu()) gameObject.SetActive(false);
    }

    private void SwipeBehaviour(InputListener.SwipeDirection dir)
    {
        
        if ( LevelManager.instance.detector.currentIt != null &&  LevelManager.instance.detector.currentIt.data.swipeDirection == dir &&  LevelManager.instance.detector.currentIt.data.interactionType == Enums.InteractionType.Swipe)
        {
            LevelManager.instance.detector.currentIt.ValidateInteraction();
        }

        if (dir == InputListener.SwipeDirection.UP)
        {
            GameManager.instance.currentCharacterInfos.power.Execute();
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
        
        if (LevelManager.instance.detector.currentIt != null &&  LevelManager.instance.detector.currentIt.data.interactionColor == color &&  LevelManager.instance.detector.currentIt.data.interactionType == Enums.InteractionType.Tap)
        {
            LevelManager.instance.detector.currentIt.ValidateInteraction();
        }
    }
    
}