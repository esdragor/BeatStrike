using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Code.Interface;
using UnityEngine;

public class InteractionDetector : MonoBehaviour
{
    public bool showDebug;
    public InteractionComponent InteractionCanTrigger = null;
    public BoxCollider collider;
    public DetectionZoneData[] detectionZoneData;
    public float tolerance = 0.01f;

    private void Start()
    {
        SetupTriggerRaw();
    }

    private void Update()
    {
        if (InteractionCanTrigger)
            SetInteractionGroup(InteractionCanTrigger);
    }

    private void SetupTriggerRaw()
    {
        float critTolerance = 0.5f;

        float ratio = 0.5f - (critTolerance / 2);
        
        detectionZoneData[0].detectionRange = ratio;
        detectionZoneData[1].detectionRange = ratio;
        detectionZoneData[2].detectionRange = critTolerance;
        detectionZoneData[3].detectionRange = ratio;
        detectionZoneData[4].detectionRange = ratio;
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

    private float offSet;

    private void OnDrawGizmosSelected()
    {
        if(!showDebug) return;
        offSet = 0;

        for (int i = 0; i < detectionZoneData.Length; i++)
        {
            Vector3 topPoint = new Vector3(transform.position.x + collider.center.x , transform.position.y + collider.center.y,
                transform.position.z  + collider.center.z + collider.size.z * 0.5f);
            Vector3 zoneCenter = new Vector3(topPoint.x, topPoint.y + 0.2f * 0.5f,
                topPoint.z - (collider.size.z * detectionZoneData[i].detectionRange) * 0.5f);

            if (i > 0)
            {
                offSet -= transform.localScale.z * detectionZoneData[i - 1].detectionRange;
                zoneCenter += new Vector3(0, 0, offSet);
            }

            Gizmos.color = detectionZoneData[i].gizmoColor;
            Gizmos.DrawCube(zoneCenter,
                new Vector3(collider.size.x, collider.size.y + 0.2f,
                    collider.size.z * detectionZoneData[i].detectionRange));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        InteractionComponent it = other.GetComponent<InteractionComponent>();

        if (it)
        {
            InteractionCanTrigger = it;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        InteractionComponent it = other.GetComponent<InteractionComponent>();
        
        if (it && InteractionCanTrigger == it) 
        {
             InteractionCanTrigger = null;
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