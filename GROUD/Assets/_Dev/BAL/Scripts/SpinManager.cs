using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class SpinManager : MonoBehaviour
{
    public static SpinManager instance;
    
    [SerializeField] private bool firstIsRed;
    [SerializeField] private Button btnLeft;
    [SerializeField] private Button btnRight;
    [SerializeField] private int nbSide = 10;
    [SerializeField, ReadOnly] private GameObject obj;
    
    public void CheckColor(bool isRed)
    {
        if (firstIsRed == isRed)
        {
            Debug.Log("Correct");
        }
        else
        {
            Debug.Log("Wrong");
        }
    }

    private void SwipeL()
    {
        obj.transform.Rotate(0, 0.3f, 0);
        firstIsRed = !firstIsRed;
    }
    
    private void SwipeR()
    {
        obj.transform.Rotate(0, -0.3f, 0);
        firstIsRed = !firstIsRed;
    }
    
    public void SetMesh(GameObject _obj)
    {
        obj = _obj;
    }
    
    private void test()
    {
        GameManager.onUpdated += SwipeL;
    }
    
    private void non()
    {
        GameManager.onUpdated -= SwipeL; 
    }
    
    private void test2()
    {
        GameManager.onUpdated += SwipeR;
    }
    
    private void non2()
    {
        GameManager.onUpdated -= SwipeR; 
    }
    
    private void Start()
    {
        btnLeft.GetComponent<InputListener>().onInputPressed += test;
        btnLeft.GetComponent<InputListener>().onInputReleased += non;
        btnRight.GetComponent<InputListener>().onInputPressed += test2;
        btnRight.GetComponent<InputListener>().onInputReleased += non2;
    }

    private void Awake()
    {
        instance = this;
    }
}