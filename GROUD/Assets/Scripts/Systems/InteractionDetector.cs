using System;
using Code.Interface;
using UnityEngine;
using Utilities;

public class InteractionDetector : MonoBehaviour
{
    public InteractionComponent currentInteraction = null;
    public BoxCollider collider;
    public DetectionZoneData[] detectionZoneData;
    public float tolerance = 0.01f;
    
    private void Update()
    {
        if (currentInteraction)
        {
            SetInteractionGroup(currentInteraction);
        }
    }
    
    private void SetInteractionGroup(InteractionComponent current)
    {
        if (detectionZoneData.Length <= 0 || current == null) return;
        
        Vector3 topPoint = new Vector3(transform.position.x + collider.center.x , transform.position.y + collider.center.y,
            transform.position.z  + collider.center.z + collider.size.z * 0.5f);
        Vector3 posInside = topPoint - current.transform.position;

        float itOffset = 0;

        for (int i = 0; i < detectionZoneData.Length; i++)
        {
            if (i > 0)
            {
                itOffset += transform.localScale.z * detectionZoneData[i - 1].detectionRange;
            }

            if (Math.Abs(posInside.z - (detectionZoneData[i].detectionRange + itOffset)) < tolerance &&
                current.successGroup != detectionZoneData[i].success)
            {
                current.SetSuccess(detectionZoneData[i].success);
            }
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        InteractionComponent it = other.GetComponent<InteractionComponent>();

        if (it)
        {
            currentInteraction = it;
            
            if (it.data.interactionType == Enums.InteractionType.Dodge)
            {
                GameLoopManager.combatManager.EnemyAttack();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        InteractionComponent it = other.GetComponent<InteractionComponent>();
        
        if (it && currentInteraction == it) 
        {
             currentInteraction = null;
        }
    }
}

[Serializable]
public struct DetectionZoneData
{
    public float detectionRange;
    public InteractionSuccess success;
    public Color gizmoColor;
}