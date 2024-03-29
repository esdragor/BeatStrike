using Code.Interface;
using UnityEngine;
using Utilities;

public class DetectorManager : MonoBehaviour
{
    public InteractionComponent currentInteraction;

    public void SetInteraction(InteractionComponent it)
    {
        if (!Equals(currentInteraction, it))
        {
            currentInteraction = it;
        }
    }
}
