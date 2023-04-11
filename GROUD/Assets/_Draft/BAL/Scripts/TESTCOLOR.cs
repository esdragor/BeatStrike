using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class TESTCOLOR : MonoBehaviour
{
    [ReadOnly] public int color;

    IEnumerator printColor()
    {
        yield return new WaitForSeconds(0.01f);
        color = Random.Range(0, 3);
        Debug.Log("color: " + color);
        GetComponent<Renderer>().material.color = color switch
        {
            0 => Color.red,
            1 => Color.blue,
            2 => Color.yellow,
            _ => Color.yellow
        };
    }
    
    private void OnEnable()
    {
        if (PatternManager.Instance == null) return;
        if (!PatternManager.Instance.isDebugMultiChannel) return;

        StartCoroutine(printColor());

    }
}
