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
            if (GameManager.instance.gameState.IsTimePlay())
            {
                //transform.position += -transform.forward * (speed * Time.deltaTime);
                if (transform.position.z < PlayerManager.instance.transform.position.z - 2f)
                {
                    if (data.interactionType is Enums.InteractionType.Dodge)
                        PlayerManager.instance.HurtPlayer();
                    PatternPoolManager.Instance.AddInteractionToPool(gameObject);
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
        public GameObject Tile;
        public Material blueMat;
        public Material redMat;
        public Material slideMat;
        [FormerlySerializedAs("FakeMat")] public Material fakeMat;
        [FormerlySerializedAs("PowerMat")] public Material powerMat;

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
                   // transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                   Tile.SetActive(false);
                    // switch (data.interactionColor)
                    // {
                    //     case InteractionKey.InteractionColor.Blue:
                    //         renderer.material = blueMat;
                    //         break;
                    //
                    //     case InteractionKey.InteractionColor.Red:
                    //         renderer.material = redMat;
                    //         break;
                    // }

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
                case Enums.InteractionType.Power:
                    //transform.localScale = new Vector3(1.8f, 0.8f, 0.8f);
                    renderer.material = powerMat;
                    break;
            }
        }

        public virtual void ValidateInteraction(bool isPerfect = false)
        {
            PlayerManager.onInteractionSuccess?.Invoke(successGroup);
            PlayerManager.instance.OnInteractionSuccess(successGroup, data.interactionType);

            LevelManager.instance.detector.InteractionCanTrigger = null;

            PatternPoolManager.Instance.AddInteractionToPool(gameObject);
        }
    }
}