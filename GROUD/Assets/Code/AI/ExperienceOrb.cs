using UnityEngine;
using Utilities;

namespace Code.AI
{
    public class ExperienceOrb : MonoBehaviour
    {
        public float speed = 3f;
        public InteractionKey dataKey;
        
        private InteractionComponent interactionComponent;

        private void Awake()
        {
            transform.Rotate(0,180,0);
        }

        private void Update()
        {
        }

        public void OnReachedPatternPoint()
        {
            switch (dataKey.interactionType)
            {
                case Enums.InteractionType.Tap:
                    GameObject interactionObj = transform.GetChild(0).gameObject;
                    interactionComponent = interactionObj.GetComponent<InteractionComponent>();
                    TapInteraction tapIn = (TapInteraction)interactionComponent;
                    interactionComponent.speed = speed / transform.position.z;
                 //   interactionComponent.ActivateInteraction();
                    tapIn.SetData(dataKey);
                    break;
        
                case Enums.InteractionType.Hold:
                    break;
        
                case Enums.InteractionType.Slide:
                    break;
        
                case Enums.InteractionType.Spam:
                    break;
            }


          //  if (interactionComponent != null) interactionComponent.StartInteraction();
        }

        public void OnReachedPlayers()
        {
            PatternPoolManager.Instance.AddCircleToPool(gameObject);
        }
    }
}