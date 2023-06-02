using Code.Interface;
using UnityEngine;
using Utilities;

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
            if (interaction.data.interactionType == Enums.InteractionType.Dodge && success == InteractionSuccess.Perfect)
            {
                GameLoopManager.combatManager.EnemyAttack();
            }
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
