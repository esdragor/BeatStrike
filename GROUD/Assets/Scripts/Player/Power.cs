using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Player
{
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
            PlayerManager.instance.matRune.SetFloat("_AbilityProgress", 0);
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
            PlayerManager.instance.vfxManager.PlaySFX("FailCombo");
        }
        

        public InteractionSuccess Execute(ScreenListener.SwipeDirection currentSwipeDirection)
        {
            if (currentSwipeDirection == combo[currentCombo])
            {
                ComboPrinter.UpdateCombo();
                currentCombo++;
                ComboPrinter.UpdateMeter(currentCombo);
                PlayerManager.instance.matRune.SetFloat("_AbilityProgress", currentCombo);
                if (currentCombo == nbCombo)
                {
                    OnSuccessAction?.Invoke();
                    currentCombo = 0;
                    return InteractionSuccess.Perfect;
                }
                if (currentCombo == nbCombo - 1)
                {
                    PlayerManager.instance.vfxManager.PlaySFX("LastCombo");
                }
                return InteractionSuccess.Ok;
            }
            OnUnsuccess();
            return InteractionSuccess.Fail;
        }
    }
}