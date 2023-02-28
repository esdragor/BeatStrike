using Code.AI;
using UnityEngine;

public class PatternTriggerEvent : MonoBehaviour
{
   private void OnTriggerEnter(Collider other)
   {
      ExperienceOrb experienceOrb = other.GetComponent<ExperienceOrb>();

      if (experienceOrb)
      {
         experienceOrb.OnReachedPatternPoint();
      }
   }
}
