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
        public Image rendererImage;
        public Sprite upSprite;
        public Sprite downSprite;
        public Sprite leftSprite;
        public Sprite rightSprite;
        public Material fakeMat;

        public void SetSuccess(InteractionSuccess itSuccess)
        {
            successGroup = itSuccess;
        }

        private void SetVisualAndColor()
        {
            float scale = 0.75f;
            transform.localScale = new Vector3(scale, scale, scale);
            rendererImage.enabled = false;
            switch (data.interactionType)
            {
                case Enums.InteractionType.Attack:
                    break;

                case Enums.InteractionType.Dodge:
                    rendererImage.enabled = true;
                    rendererImage.sprite = data.swipeDirection switch
                    {
                        ScreenListener.SwipeDirection.LEFT => leftSprite,
                        ScreenListener.SwipeDirection.RIGHT => rightSprite,
                        ScreenListener.SwipeDirection.UP => upSprite,
                        ScreenListener.SwipeDirection.DOWN => downSprite,
                        _ => rendererImage.sprite
                    };
                    break;
                case Enums.InteractionType.Fake:
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