using Code.Interface;
using UnityEngine;
using Utilities;

public class PlayerTriggerEvent : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        InteractionComponent it = other.GetComponent<InteractionComponent>();
        if (false == true)
            SoundManager.PlayRandomBadNotePlayer((int)GameManager.instance.Bpm);
        if (it)
        {
            if (it.data.interactionType != Enums.InteractionType.Attack)
            {
                PlayerManager.instance.HurtPlayer(GameLoopManager.combatManager.getAttackData());
            }

            GameLoopManager.interactionPool.AddInteractionToPool(it.gameObject);
        }
    }
}