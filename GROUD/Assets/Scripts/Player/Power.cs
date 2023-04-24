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
            ratioCD = 1;
            powerCooldown = 1;
        }
        public abstract void OnUnset();
        public abstract void Execute();
    }
}