using System;
using System.Collections.Generic;
using Code.Interface;
using UnityEngine;
using Utilities;

public class InputManager : MonoBehaviour
{
    public ScreenListener screenListener;
    private PlayerManager playerManager = null;

    void OnEnable()
    {
        screenListener.onInputPressed += TapBehaviour;
        screenListener.onSwipeDetected += SwipeBehaviour;
        GameManager.gameState.OnEngineStateChanged += UpdateEnable;
    }

    private void OnDisable()
    {
        screenListener.onInputPressed -= TapBehaviour;
        screenListener.onSwipeDetected -= SwipeBehaviour;
        GameManager.gameState.OnEngineStateChanged -= UpdateEnable;
    }

    private void UpdateEnable(Enums.EngineState obj)
    {
        switch (obj)
        {
            case Enums.EngineState.Game:
                gameObject.SetActive(true);
                break;

            case Enums.EngineState.Menu:
                //   gameObject.SetActive(false);
                break;
        }
    }

    private void LaunchAnimationSwipe(ScreenListener.SwipeDirection dir)
    {
        if (!playerManager) playerManager = PlayerManager.instance;
        if (!playerManager) return;

        switch (dir)
        {
            case ScreenListener.SwipeDirection.UP:
                if (GameLoopManager.instance.isDefPhase)
                    playerManager.animator.SetTrigger("DefUp");
                else
                    playerManager.animator.SetTrigger("AttackUp");
                playerManager.vfxManager.PlaySFX("Attack", dir);
                break;
            case ScreenListener.SwipeDirection.DOWN:
                if (GameLoopManager.instance.isDefPhase)
                    playerManager.animator.SetTrigger("DefDown");
                else
                    playerManager.animator.SetTrigger("AttackDown");
                playerManager.vfxManager.PlaySFX("Attack", dir);

                break;
            case ScreenListener.SwipeDirection.LEFT:
                if (GameLoopManager.instance.isDefPhase)
                    playerManager.animator.SetTrigger("DefLeft");
                else
                    playerManager.animator.SetTrigger("AttackLeft");
                playerManager.vfxManager.PlaySFX("Attack", dir);

                break;
            case ScreenListener.SwipeDirection.RIGHT:
                if (GameLoopManager.instance.isDefPhase)
                    playerManager.animator.SetTrigger("DefRight");
                else
                    playerManager.animator.SetTrigger("AttackRight");
                playerManager.vfxManager.PlaySFX("Attack", dir);

                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(dir), dir, null);
        }
    }

    private void SwipeBehaviour(ScreenListener.SwipeDirection dir)
    {
        if (GameManager.gameState.IsEngineMenu() && !UIManager.instance.tutorial.ended &&
            dir == ScreenListener.SwipeDirection.LEFT)
        {
            UIManager.instance.tutorial.DrawNext();
        }

        InteractionComponent it = GameLoopManager.instance.detector.currentInteraction;
        if (!it) return;

        LaunchAnimationSwipe(dir);

        switch (it.data.interactionType)
        {
            case Enums.InteractionType.Attack:
                it.ValidateInteraction(dir);
                break;
            case Enums.InteractionType.Dodge:
                if (it.data.swipeDirection == dir)
                {
                    it.ValidateInteraction(dir);
                    playerManager.vfxManager.PlaySFX("Dodge", dir);
                }

                break;
            case Enums.InteractionType.Fake:

                break;
        }
        SoundManager.PlayRandomGoodNotePlayer((int)GameManager.instance.Bpm);
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