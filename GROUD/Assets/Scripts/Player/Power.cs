using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Player
{
    // public abstract class Power
    // {
    //     public Sprite powerSprite;
    //     protected float powerCooldown;
    //     protected float ratioCD = 0;
    //
    //
    //     protected bool onCooldown = false;
    //     protected bool powerRunning = false;
    //     
    //     public abstract void OnSet();
    //     protected void OnSetBase()
    //     {
    //         ratioCD = 1;
    //         powerCooldown = 1;
    //     }
    //     public abstract void OnUnset();
    //     public abstract void Execute();
    // }


    public abstract class ComboPower
    {
        protected int nbCombo = 4;
        protected ScreenListener.SwipeDirection[] combo;
        protected int currentCombo = 0;
        
        public Action OnSuccessAction;

        public virtual void OnSuccess(float value = 0)
        {
            PlayerManager.instance.vfxManager.PlaySFX("Ability");
        }

        public void OnSetBase()
        {
            combo = new ScreenListener.SwipeDirection[nbCombo];
            currentCombo = 0;
            for (int i = 0; i < nbCombo; i++)
            {
                ScreenListener.SwipeDirection rdnSlide = (ScreenListener.SwipeDirection)UnityEngine.Random.Range(1,
                    Enum.GetNames(typeof(ScreenListener.SwipeDirection)).Length);
                combo[i] = rdnSlide;
            }
            ComboPrinter.PrintNewCombo(combo);
            ComboPrinter.UpdateMeter(currentCombo);
        }

        public List<ScreenListener.SwipeDirection> RemainingCombo()
        {
            List<ScreenListener.SwipeDirection> remainingCombo = new List<ScreenListener.SwipeDirection>();
            for (int i = currentCombo; i < nbCombo; i++)
            {
                remainingCombo.Add(combo[i]);
            }
            return remainingCombo;
        }

        private void OnUnsuccess()
        {
            // yes
        }

        public InteractionSuccess Execute(ScreenListener.SwipeDirection currentSwipeDirection)
        {
            if (currentSwipeDirection == combo[currentCombo])
            {
                ComboPrinter.UpdateCombo();
                currentCombo++;
                float cb = currentCombo;
                ComboPrinter.UpdateMeter(currentCombo);
                if (currentCombo == nbCombo)
                {
                    OnSuccessAction?.Invoke();
                    currentCombo = 0;
                    return InteractionSuccess.Perfect;
                }
                return InteractionSuccess.Ok;
            }
            OnUnsuccess();
            return InteractionSuccess.Fail;
        }
    }
}