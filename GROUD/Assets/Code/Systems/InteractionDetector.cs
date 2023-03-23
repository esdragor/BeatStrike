using System;
using System.Collections.Generic;
using UnityEngine;

public class InteractionDetector : MonoBehaviour
{
    public Action<InteractionComponent> OnInteractionAdded;
    public Action<InteractionComponent> OnInteractionRemoved;
    private List<InteractionComponent> interactions;
    public DetectionZoneData[] detectionZoneData;
    public float tolerance = 0.01f;

    public bool IsInteractionEmpty()
    {
        return interactions.Count <= 0;
    }

    private void Awake()
    {
        interactions = new List<InteractionComponent>();
    }

    private void Update()
    {
        if(!IsInteractionEmpty()) SetInteractionGroup();
    }

    public InteractionComponent PeekInteraction()
    {
        InteractionComponent it = IsInteractionEmpty() ? null : interactions[0];
        if (it != null) interactions.Remove(it);
        return it;
    }

    private void SetInteractionGroup()
    {
        if(detectionZoneData.Length <= 0) return;

        
        foreach (InteractionComponent it in interactions)
        {
            Vector3 topPoint = new Vector3(transform.position.x, transform.position.y, transform.position.z + transform.localScale.z * 0.5f);
            float itDistanceToTop = Vector3.Distance(topPoint, it.transform.position);
            
            float itOffset = 0;

            for (int i = 0; i < detectionZoneData.Length; i++)
            {
                if (i > 0)
                {
                    itOffset += detectionZoneData[i - 1].detectionRange;
                }

                if (Math.Abs(((detectionZoneData)[i].detectionRange + itOffset) - itDistanceToTop) < tolerance)
                {
                   // Debug.Log($"Distance { itDistanceToTop } | DetectionZone {detectionZoneData[i].detectionRange + itOffset} | Success { detectionZoneData[i].success }");
                    it.SetSuccess(detectionZoneData[i].success);
                }
            }
        }
    }

    private float offSet;
    private void OnDrawGizmosSelected()
    {
        offSet = 0;
        for (int i = 0; i < detectionZoneData.Length; i++)
        {
            Vector3 topPoint = new Vector3(transform.position.x, transform.position.y, transform.position.z + transform.localScale.z * 0.5f);
            Vector3 zoneCenter = new Vector3(topPoint.x, topPoint.y + 0.2f * 0.5f, topPoint.z - detectionZoneData[i].detectionRange * 0.5f);
            
            if (i > 0)
            {
                offSet -= detectionZoneData[i - 1].detectionRange;
                zoneCenter += new Vector3(0, 0,offSet );
            }
            
            Gizmos.color = detectionZoneData[i].gizmoColor;
            Gizmos.DrawCube(zoneCenter, new Vector3(transform.localScale.x, transform.localScale.y + 0.2f, detectionZoneData[i].detectionRange));
        }     
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
            OnInteractionRemoved?.Invoke(it);
        }
        
    }
}

[Serializable] public struct DetectionZoneData
{
    public float detectionRange;
    public InteractionSuccess success;
    public Color gizmoColor;
}
