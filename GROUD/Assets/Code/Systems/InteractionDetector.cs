using System;
using System.Collections.Generic;
using UnityEngine;

public class InteractionDetector : MonoBehaviour
{
    public Action<InteractionComponent> OnInteractionAdded;
    public List<InteractionComponent> interactions;

    public bool IsInteractionEmpty()
    {
        return interactions.Count <= 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        InteractionComponent it = other.GetComponent<InteractionComponent>();

        if (it && !interactions.Contains(it))
        {
            interactions.Add(it);
            OnInteractionAdded?.Invoke(it);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        InteractionComponent it = other.GetComponent<InteractionComponent>();

        if (it && interactions.Contains(it))
        {
            interactions.Remove(it);
        }
        
    }
}
