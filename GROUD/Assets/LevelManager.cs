using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
}

[Serializable] public class World
{
    public GameObject[] visuals;
    public LevelData[] levelDatas;
}
