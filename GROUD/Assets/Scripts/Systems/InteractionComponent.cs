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
            if (GameManager.instance.gameState.IsTimePlay())
            {
                transform.position += -transform.forward * (speed * Time.deltaTime);
                if (transform.position.z < PlayerManager.instance.transform.position.z - 2f)
                {
                    if (data.interactionType is Enums.InteractionType.Dodge or Enums.InteractionType.Fake)
                        PlayerManager.instance.HurtPlayer();
                    PatternPoolManager.Instance.AddInteractionToPool(gameObject);
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
                case Enums.InteractionType.Attack:
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

                case Enums.InteractionType.Dodge:
                    transform.localScale = new Vector3(1.8f, 0.8f, 0.8f);
                    renderer.material = slideMat;

                    break;
            }
        }

        public virtual void ValidateInteraction(bool isPerfect = false)
        {
            PlayerManager.onInteractionSuccess?.Invoke(successGroup);
            PlayerManager.instance.OnInteractionSuccess(successGroup, data.interactionType);

            LevelManager.instance.detector.InteractionCanTrigger.Remove(this);

            PatternPoolManager.Instance.AddInteractionToPool(gameObject);
        }
    }
}