using UnityEngine;
using Utilities;

namespace Code.Interface
{
    public class InteractionComponent : MonoBehaviour
    {
        public InteractionKey data;
        public float speed = 3f;
        public InteractionSuccess successGroup;

        private void Update()
        {
            if (GameManager.gameState.IsTimePlay())
            {
                transform.position += -transform.forward * (speed * Time.deltaTime);
                if (transform.position.z < PlayerManager.instance.transform.position.z - 2f)
                {
                    LevelManager.interactionPool.AddInteractionToPool(gameObject);
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

        public MeshRenderer renderer;
        public Material blueMat;
        public Material redMat;
        public Material slideMat;

        public void SetSuccess(InteractionSuccess itSuccess)
        {
            successGroup = itSuccess;
        }

        private void SetVisualAndColor()
        {
            switch (data.interactionType)
            {
                case Enums.InteractionType.Tap:
                    transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

                    switch (data.interactionColor)
                    {
                        case InteractionKey.InteractionColor.Blue:
                            renderer.material = blueMat;
                            break;

                        case InteractionKey.InteractionColor.Red:
                            renderer.material = redMat;
                            break;
                    }

                    break;

                case Enums.InteractionType.Swipe:
                    transform.localScale = new Vector3(1.8f, 0.8f, 0.8f);
                    renderer.material = slideMat;

                    break;
            }
        }

        public virtual void ValidateInteraction(bool isPerfect = false)
        {
            if (GameManager.gameState.IsLevelExploration())
            {
            }
            PlayerManager.onInteractionSuccess?.Invoke(successGroup);
            PlayerManager.instance.OnInteractionSuccess(PlayerManager.instance.justPerfectEnabled
                ? InteractionSuccess.Perfect
                : successGroup);

            LevelManager.instance.detector.InteractionCanTrigger.Remove(this);

            LevelManager.interactionPool.AddInteractionToPool(gameObject);
        }
    }
}