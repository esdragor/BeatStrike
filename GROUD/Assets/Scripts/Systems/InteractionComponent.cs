using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using Utilities;

namespace Code.Interface
{
    public class InteractionComponent : MonoBehaviour
    {
        public InteractionKey data;
        public InteractionSuccess successGroup;

        private float speedMultiplierOffset = 1.5f;

        public GameObject attack;
        public GameObject down;
        public GameObject left;
        public GameObject right;
        public GameObject up;
        
        public ParticleSystem depopVFX;
        public ParticleSystem popVFX;
        
        private float speed = 6f;
        private bool isPooling = false;
        
        
        private void Update()
        {
            if (!UIManager.instance.isPaused && !isPooling)
            { 
                transform.position += Vector3.back * (speed * GameManager.instance.fakeDeltaTime);
            }
        }

        public void SetData(InteractionKey interactionKey)
        {
            data = interactionKey;
            successGroup = InteractionSuccess.Perfect;
            
            SetVisualAndColor();
        }

        public void SetSuccess(InteractionSuccess itSuccess)
        {
            successGroup = itSuccess;
        }

        private void SetVisualAndColor()
        {
            transform.localScale = new Vector3(1f, 1f, 1f);

            switch (data.interactionType)
            {
                case Enums.InteractionType.Attack:
                    attack.SetActive(true);
                    break;

                case Enums.InteractionType.Dodge:
                    
                    switch (data.swipeDirection)
                    {
                        case ScreenListener.SwipeDirection.UP:
                            up.SetActive(true);
                            break;
                        
                        case ScreenListener.SwipeDirection.DOWN:
                            down.SetActive(true);
                            break;
                        
                        case ScreenListener.SwipeDirection.LEFT:
                            left.SetActive(true);
                            break;
                            
                            case ScreenListener.SwipeDirection.RIGHT:
                            right.SetActive(true);
                                break;
                    }
                    break;
                
                case Enums.InteractionType.Fake:
                    break;
            }
        }

        public virtual void ValidateInteraction(ScreenListener.SwipeDirection dir, bool isPerfect = false)
        {
            PlayerManager.onInteractionSuccess?.Invoke(successGroup);
            PlayerManager.instance.OnInteractionSuccess(successGroup, data.interactionType, dir);

            GameLoopManager.instance.detector.currentInteraction = null;
            Depop();
        }

        private void Disable()
        {
            gameObject.SetActive(false);
            
            left.SetActive(false);
            right.SetActive(false);
            up.SetActive(false);
            down.SetActive(false);
            attack.SetActive(false);
        }
        
        public void Depop()
        {
            left.SetActive(false);
            right.SetActive(false);
            up.SetActive(false);
            down.SetActive(false);
            attack.SetActive(false);
            
            if (gameObject.activeSelf)
            {
                WaitForDisable();
            }
        }

        public async void WaitForDisable()
        {
            depopVFX.Play();
            popVFX.Play();
            isPooling = true;

            while (depopVFX.isPlaying && popVFX.isPlaying)
            {
                await Task.Yield();
            }

            GameLoopManager.interactionPool.AddInteractionToPool(gameObject);
            isPooling = false;
        }
        

        private void OnDisable()
        {
           Disable();
        }
    }
}