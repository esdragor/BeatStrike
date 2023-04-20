using System;
using UnityEngine;

namespace Code.Player
{
    public abstract class Power
    {
        public Sprite powerSprite;
        public abstract void OnSet();
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