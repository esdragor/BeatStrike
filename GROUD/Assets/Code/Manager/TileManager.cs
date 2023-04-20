using System.Collections;
using System.Collections.Generic;
using Code.Interface;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    private static TileManager instance;
    private List<Transform> currentTile = new();
    
    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public static void AddTile(Transform interactionComponent)
    {
        instance.currentTile.Add(interactionComponent);
    }
    
    public static void RemoveTile(Transform interactionComponent)
    {
        instance.currentTile.Remove(interactionComponent);
    }
    
    public static List<Transform> GetCurrentTile()
    {
        return instance.currentTile;
    }
}