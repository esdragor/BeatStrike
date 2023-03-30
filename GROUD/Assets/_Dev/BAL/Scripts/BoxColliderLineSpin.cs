using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxColliderLineSpin : MonoBehaviour
{
    [SerializeField] private int index;

    private void OnTriggerEnter(Collider other)
    {
        //SpinManager.instance.CheckColor(-0.25f + index * 0.25f, other.GetComponent<TESTCOLOR>().color);
        PatternPoolManager.Instance.AddInteractionToPool(other.gameObject);
    }
}