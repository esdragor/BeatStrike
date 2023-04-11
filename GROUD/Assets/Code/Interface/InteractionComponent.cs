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
                    transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

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
                    renderer.material = slideMat;

                    break;
            }
        }

        public virtual void ValidateInteraction(bool isPerfect = false)
        {
            PlayerStats stats = PlayerManager.instance.currentStats;
            if (GameManager.instance.gameState.IsLevelExploration())
            {
                PlayerManager.instance.AddExperience(10f * stats.experienceFactor);
            }
            else
            {
                float damage = stats.damage;
                if (isPerfect)
                    damage *= stats.critRate;
                BossManager.instance.AddDamageToPool(damage);
            }

            PlayerManager.instance.OnInteractionSuccess(PlayerManager.instance.powerIsRunning
                ? InteractionSuccess.Perfect
                : successGroup);

            LevelManager.instance.detector.InteractionCanTrigger.Remove(this);

            PatternPoolManager.Instance.AddInteractionToPool(gameObject);
        }

        public virtual void HurtPlayer()
        {
            PlayerManager.instance.TakeDamage(1f);

            PatternPoolManager.Instance.AddInteractionToPool(gameObject);
        }
    }
}