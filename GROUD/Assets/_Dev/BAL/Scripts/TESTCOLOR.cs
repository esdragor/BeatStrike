using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TESTCOLOR : MonoBehaviour
{
    public int color;

    private void OnEnable()
    {
        color = Random.Range(0, 3);
        GetComponent<Renderer>().material.color = color switch
        {
            0 => Color.red,
            1 => Color.blue,
            _ => Color.yellow
        };
    }
}
