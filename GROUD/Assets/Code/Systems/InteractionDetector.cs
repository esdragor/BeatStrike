using System;
using UnityEngine;

public class InteractionDetector : MonoBehaviour
{
    public Action<InteractionComponent> OnInteractionAdded;
    public Action<InteractionComponent> OnInteractionRemoved;
    public InteractionComponent currentIt;
    public DetectionZoneData[] detectionZoneData;
    public float tolerance = 0.01f;

    private void Update()
    {
        if (currentIt != null) SetInteractionGroup();
    }

    private void SetInteractionGroup()
    {
        if (detectionZoneData.Length <= 0 || currentIt == null) return;

        Vector3 topPoint = new Vector3(transform.position.x, transform.position.y,
            transform.position.z + transform.localScale.z * 0.5f);
        Vector3 posInside = topPoint - currentIt.transform.position;

        float itOffset = 0;

        for (int i = 0; i < detectionZoneData.Length; i++)
        {
            if (i > 0)
            {
                itOffset += transform.localScale.z * detectionZoneData[i - 1].detectionRange;
            }

            if (Math.Abs(posInside.z - (detectionZoneData[i].detectionRange + itOffset)) < tolerance &&
                currentIt.successGroup != detectionZoneData[i].success)
            {
                //Debug.Log($"Distance was {posInside.z} so it's {detectionZoneData[i].success}");
                currentIt.SetSuccess(detectionZoneData[i].success);
            }
        }
    }

    private float offSet;

    private void OnDrawGizmosSelected()
    {
        offSet = 0;

        for (int i = 0; i < detectionZoneData.Length; i++)
        {
            Vector3 topPoint = new Vector3(transform.position.x, transform.position.y,
                transform.position.z + transform.localScale.z * 0.5f);
            Vector3 zoneCenter = new Vector3(topPoint.x, topPoint.y + 0.2f * 0.5f,
                topPoint.z - (transform.localScale.z * detectionZoneData[i].detectionRange) * 0.5f);

            if (i > 0)
            {
                offSet -= transform.localScale.z * detectionZoneData[i - 1].detectionRange;
                zoneCenter += new Vector3(0, 0, offSet);
            }

            Gizmos.color = detectionZoneData[i].gizmoColor;
            Gizmos.DrawCube(zoneCenter,
                new Vector3(transform.localScale.x, transform.localScale.y + 0.2f,
                    transform.localScale.z * detectionZoneData[i].detectionRange));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        InteractionComponent it = other.GetComponent<InteractionComponent>();

        if (it)
        {
            if (PlayerManager.instance.immortality)
            {
                PlayerManager.instance.OnInteractionSuccess(InteractionSuccess.Ok);
        
                PatternPoolManager.Instance.AddInteractionToPool(other.gameObject);
            }
            currentIt = it;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        InteractionComponent it = other.GetComponent<InteractionComponent>();

        if (it && currentIt == it)
        {
            currentIt = null;
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