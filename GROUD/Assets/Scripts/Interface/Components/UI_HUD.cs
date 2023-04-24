using UnityEngine;

public class UI_HUD : MonoBehaviour
{
   public UI_PlayerHealth playerHealth;
   
   public void EnableHUD()
   {
      gameObject.SetActive(true);
   }

   public void DisableHUD()
   {
      gameObject.SetActive(false);
   }

   public void StartLevel()
   {
      LevelManager.instance.StartLevel();
   }
}
