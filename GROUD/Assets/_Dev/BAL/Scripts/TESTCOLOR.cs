using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TESTCOLOR : MonoBehaviour
{
    public bool isRed;

    private void OnEnable()
    {
        isRed = Random.Range(0, 2) == 0;
    }
}
