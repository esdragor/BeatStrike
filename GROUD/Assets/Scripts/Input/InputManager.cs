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
        if (GameManager.instance.gameState.IsEngineMenu()) gameObject.SetActive(false);
    }

    private void SwipeBehaviour(ScreenListener.SwipeDirection dir)
    {
        InteractionComponent it = LevelManager.instance.detector.InteractionCanTrigger;
        if (!it) return;

        switch (it.data.interactionType)
        {
            case Enums.InteractionType.Attack:
                it.ValidateInteraction();
                break;
            case Enums.InteractionType.Dodge:
                if (it.data.swipeDirection == dir)
                {
                    it.ValidateInteraction();
                }
                break;
            case Enums.InteractionType.Fake:

                break;
        }
    }

    private void TapBehaviour(ScreenListener.TouchSide touchSide)
    {
    /*InteractionKey.InteractionColor color;

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
        }*/
    }
}