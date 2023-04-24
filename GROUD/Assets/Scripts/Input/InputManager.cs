using System.Collections.Generic;
using Code.Interface;
using UnityEngine;
using Utilities;

public class InputManager : MonoBehaviour
{
    public ScreenListener screenListener;
    private InteractionComponent selectedSwipeIt;

    void OnEnable()
    {
        screenListener.onInputPressed += TapBehaviour;
        screenListener.onSwipeDetected += SwipeBehaviour;
    }
    private void OnDisable()
    {
        screenListener.onInputPressed -= TapBehaviour;
        screenListener.onSwipeDetected -= SwipeBehaviour;
    }

    private void Start()
    {
        if(GameManager.gameState.IsEngineMenu()) gameObject.SetActive(false);
    }

    private void SwipeBehaviour(ScreenListener.SwipeDirection dir)
    {
        List<InteractionComponent> itList = LevelManager.instance.detector.InteractionCanTrigger;
        
        for (int i = 0; i < itList.Count; i++)
        { 
            if(itList[i].data.swipeDirection == dir && itList[i].data.interactionType == Enums.InteractionType.Dodge)
            {
                itList[i].ValidateInteraction();
            }
        }

        if (dir == ScreenListener.SwipeDirection.UP)
        {
            GameManager.instance.power.Execute();
        }
        
    }

    private void TapBehaviour(ScreenListener.TouchSide touchSide)
    {
        InteractionKey.InteractionColor color;
        
        switch (touchSide)
        {
            case ScreenListener.TouchSide.LEFT:
                color = InteractionKey.InteractionColor.Blue;
                break;
            
            case ScreenListener.TouchSide.RIGHT:
                color = InteractionKey.InteractionColor.Red;
                break;
            default:
                return;
        }
        
        List<InteractionComponent> itList = LevelManager.instance.detector.InteractionCanTrigger;
        
        for (int i = 0; i < itList.Count; i++)
        {
            if (itList[i].data.interactionColor == color &&
                itList[i].data.interactionType == Enums.InteractionType.Attack)
            {
                itList[i].ValidateInteraction();
            }
        }
        
    }
    
}