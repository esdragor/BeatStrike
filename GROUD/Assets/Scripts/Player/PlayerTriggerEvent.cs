using Code.Interface;
using UnityEngine;
using Utilities;

public class PlayerTriggerEvent : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        InteractionComponent it = other.GetComponent<InteractionComponent>();

        if (it)
        {
            if (it.data.interactionType != Enums.InteractionType.Dodge)
            {
                PlayerManager.instance.HurtPlayer();
            }
        }
    }
}
