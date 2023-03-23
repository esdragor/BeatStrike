using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public UI_Announcer announcer;
    
    private void Awake()
    {
        instance = this;
    }
}
