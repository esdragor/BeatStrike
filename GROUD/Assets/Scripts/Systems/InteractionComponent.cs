using System;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace Code.Interface
{
    public class InteractionComponent : MonoBehaviour
    {
        public InteractionKey data;
        public float speed = 3f;
        public InteractionSuccess successGroup;

        private float speedMultiplierOffset = 1.5f;

        private void Update()
        {
            if (GameManager.gameState.IsTimePlay())
            { 
                transform.position += Vector3.back * (speed * Time.deltaTime);
                
                if (transform.position.z < PlayerManager.instance.transform.position.z - 2f)
                {
                    if (data.interactionType is Enums.InteractionType.Dodge)
                        PlayerManager.instance.HurtPlayer();
                    GameLoopManager.interactionPool.AddInteractionToPool(gameObject);
                    StreakManager.RemoveStreak();
                }
            }
        }

        public void SetData(InteractionKey interactionKey)
        {
            data = interactionKey;
            successGroup = InteractionSuccess.Perfect;
            SetVisualAndColor();
        }

    
        public GameObject attack;
        public GameObject down;
        public GameObject left;
        public GameObject right;
        public GameObject up;

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

        public virtual void ValidateInteraction(bool isPerfect = false)
        {
            PlayerManager.onInteractionSuccess?.Invoke(successGroup);
            PlayerManager.instance.OnInteractionSuccess(successGroup, data.interactionType);

            GameLoopManager.instance.detector.InteractionCanTrigger = null;
            
            left.SetActive(false);
            right.SetActive(false);
            up.SetActive(false);
            down.SetActive(false);
            attack.SetActive(false);

            GameLoopManager.interactionPool.AddInteractionToPool(gameObject);
        }
    }
}