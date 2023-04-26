using UnityEngine;

public class UI_Enemy : MonoBehaviour
{
        public UI_EnemyHealth enemyHealth;
        
        public void EnableEnemyHealth(bool e)
        { 
                gameObject.SetActive(e);
        }
}