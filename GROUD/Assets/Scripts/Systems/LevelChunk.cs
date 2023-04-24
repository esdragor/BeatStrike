using UnityEngine;

public class LevelChunk : MonoBehaviour
{
    [Header("Room")] 
    public Transform roomPosition;
    
    [Header("Corridor")]
    public Transform[] corridorPositions;
    private int index;

    public Vector3 GetCorridorPosition()
    {
        int oldIndex = index;
        index++;
        
        return corridorPositions[oldIndex].position;
    }
}
