using System;
using UnityEngine;

namespace Code.Player
{
    public abstract class Power
    {
        public Sprite powerSprite;
        protected float powerCooldown;
        protected float ratioCD = 0;


        protected bool onCooldown = false;
        protected bool powerRunning = false;
        
        public abstract void OnSet();
        protected void OnSetBase()
        {
            ratioCD = PlayerManager.instance.currentStats.intelligence;
            powerCooldown = PlayerManager.instance.currentStats.stamina;
        }
        public abstract void OnUnset();
        public abstract void Execute();

        public bool UnequipOnPlayer(PowerDescription powerDescription)
        {
            CharacterInfos ch = GameManager.instance.currentCharacterInfos;

            ch.power = null;

            if (MainMenuManager.instance == null) return false;
            MainMenuManager.instance.SetUnPowerImage(powerDescription);
            return false;
        }

        public void EquipOnPlayer(PowerDescription currentPower)
        {
            if (!currentPower.OnEquip)
            {
                
                CharacterInfos ch = GameManager.instance.currentCharacterInfos;

                ch.power = currentPower.Power;
                
                currentPower.OnEquip = true;
                OnSet();
                MainMenuManager.instance.SetPowerImage(currentPower);
            }
        }
    }
}