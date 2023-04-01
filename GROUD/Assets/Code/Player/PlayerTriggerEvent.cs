using Code.Interface;
using UnityEngine;

public class PlayerTriggerEvent : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        InteractionComponent it = other.GetComponent<InteractionComponent>();

        if (it)
        {
            it.HurtPlayer();
        }
    }
}
