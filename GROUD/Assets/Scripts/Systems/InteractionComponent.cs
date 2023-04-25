using UnityEngine;
using UnityEngine.Serialization;
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
                //transform.position += -transform.forward * (speed * Time.deltaTime);
                if (transform.position.z < PlayerManager.instance.transform.position.z - 2f)
                {
                    if (data.interactionType is Enums.InteractionType.Dodge)
                        PlayerManager.instance.HurtPlayer();
                    GameLoopManager.interactionPool.AddInteractionToPool(gameObject);
                    StreakManager.RemoveStreak();
                }
                renderer.material.mainTextureOffset += new Vector2(0, Time.deltaTime * speedMultiplierOffset);
            }
        }

        public void SetData(InteractionKey interactionKey)
        {
            data = interactionKey;
            successGroup = InteractionSuccess.Perfect;
            SetVisualAndColor();
        }

        public MeshRenderer renderer;
        public Material blueMat;
        public Material redMat;
        public Material whiteMat;
        public Material fakeMat;

        public void SetSuccess(InteractionSuccess itSuccess)
        {
            successGroup = itSuccess;
        }

        private void SetVisualAndColor()
        {
            transform.localScale = new Vector3(1.8f, 0.8f, 0.8f);
            switch (data.interactionType)
            {
                case Enums.InteractionType.Attack:
                    renderer.material = whiteMat;

                    break;

                case Enums.InteractionType.Dodge:
                    //transform.localScale = new Vector3(1.8f, 0.8f, 0.8f);
                    switch (data.swipeDirection)
                    {
                        case ScreenListener.SwipeDirection.LEFT:
                            renderer.material = blueMat;
                            break;

                        case ScreenListener.SwipeDirection.RIGHT:
                            renderer.material = redMat;
                            break;
                    }
                    break;
                case Enums.InteractionType.Fake:
                    //transform.localScale = new Vector3(1.8f, 0.8f, 0.8f);
                    renderer.material = fakeMat;
                    break;
            }
        }

        public virtual void ValidateInteraction(bool isPerfect = false)
        {
            PlayerManager.onInteractionSuccess?.Invoke(successGroup);
            PlayerManager.instance.OnInteractionSuccess(successGroup, data.interactionType);

            GameLoopManager.instance.detector.InteractionCanTrigger = null;

            GameLoopManager.interactionPool.AddInteractionToPool(gameObject);
        }
    }
}