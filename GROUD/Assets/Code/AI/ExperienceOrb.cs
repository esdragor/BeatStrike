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
            transform.position += transform.forward * speed * Time.deltaTime;
        }

        public void OnReachedPatternPoint()
        {
            switch (dataKey.interactionType)
            {
                case Enums.InteractionType.Tap:
                    GameObject interactionObj = transform.GetChild(0).gameObject;
                    interactionComponent = interactionObj.GetComponent<InteractionComponent>();
                    TapInteraction tapIn = (TapInteraction)interactionComponent;
                    float speed = transform.GetComponent<ExperienceOrb>().speed;
                    interactionComponent.speed = transform.position.z / (speed * 0.75f);
                    interactionComponent.ActivateInteraction();
                    tapIn.SetData(dataKey);
                    break;
        
                case Enums.InteractionType.Hold:
                    break;
        
                case Enums.InteractionType.Slide:
                    break;
        
                case Enums.InteractionType.Spam:
                    break;
            }


            if (interactionComponent != null) interactionComponent.StartInteraction();
        }

        public void OnReachedPlayers()
        {
            PatternPoolManager.Instance.AddCircleToPool(gameObject);
            InputManager.OnFailedTouchInteraction?.Invoke();
        }
    }
}