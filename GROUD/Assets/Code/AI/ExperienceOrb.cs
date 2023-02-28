using UnityEngine;

namespace Code.AI
{
    public class ExperienceOrb : MonoBehaviour
    {
        public float speed = 3f;
        public Pattern orbPattern;
        public Vector3 specialSpawnPosition;

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
            PatternManager.Instance.StartPattern(orbPattern);
        }

        public void OnReachedPlayers()
        {
            Destroy(gameObject);
        }
    }
}