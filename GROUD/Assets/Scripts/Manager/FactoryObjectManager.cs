using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryObjectManager : MonoBehaviour
{
    private static FactoryObjectManager instance;
    
    [SerializeField] private FactoryObject[] factoryObjectReference; // 0 = common, 1 = epic
    
    private void Awake()
    {
        if (instance == null) instance = this;
    }
}
