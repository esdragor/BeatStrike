using System;
using System.Collections;
using System.Collections.Generic;
using Code.Interface;
using UnityEngine;
using Utilities;

public class TriggerLaunchEnemy : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        InteractionComponent interaction = other.GetComponent<InteractionComponent>();

        if (interaction)
        {
            if (interaction.data.interactionType == Enums.InteractionType.Dodge)
            {
                GameLoopManager.combatManager.EnemyAttack();
            }
        }
    }
}
