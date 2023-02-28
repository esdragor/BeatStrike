using UnityEngine;
using Utilities;

[CreateAssetMenu(order = 0, menuName = "Key/Interaction Key", fileName = "newInteractionKey")]
public class InteractionKey : KeyClass
{
    public float scale;
    public float tolerance;
    public float drawSpeed;
    public Vector3 spawnPosition;
    public Enums.InteractionType interactionType;
}
