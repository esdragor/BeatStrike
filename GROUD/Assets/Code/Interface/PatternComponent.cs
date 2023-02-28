using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PatternComponent : MonoBehaviour
{
    public float tolerance = 20f;
    public float speed = 0.1f;

    public abstract void OnInputSuccess();
}