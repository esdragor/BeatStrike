using Code.Interface;
using UnityEngine;

public class Detector : MonoBehaviour
{
    private DetectorManager detectorManager => GameLoopManager.instance.detector;
    public InteractionSuccess success;
    public bool isLast;
    private void OnTriggerEnter(Collider col)
    {
        InteractionComponent interaction = col.GetComponent<InteractionComponent>();

        if (interaction)
        {
            interaction.SetSuccess(success);
            detectorManager.SetInteraction(interaction);
        }
    }

    private void OnTriggerExit(Collider col)
    {
        InteractionComponent interaction = col.GetComponent<InteractionComponent>();

        if (isLast && interaction)
        {
           detectorManager.SetInteraction(null);
        }
    }
}
