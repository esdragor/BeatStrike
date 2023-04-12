using System;
using System.Threading.Tasks;
using UnityEngine;

public abstract class Power
{
    public abstract void OnSet();
    public abstract void OnUnset();
    public abstract void Execute();
}