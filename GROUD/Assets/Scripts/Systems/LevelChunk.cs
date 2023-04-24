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
        
        if (index < corridorPositions.Length)
        {
            int oldIndex = index;
            index++;
            
            return corridorPositions[oldIndex].position;
        }
        else
        {
            return Vector3.zero;
        }
    }
}
