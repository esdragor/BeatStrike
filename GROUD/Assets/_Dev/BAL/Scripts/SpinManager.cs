using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class SpinManager : MonoBehaviour
{
    public static SpinManager instance;

    [SerializeField] private Button btnLeft;
    [SerializeField] private Button btnRight;
    [SerializeField, ReadOnly] private int nbSide = 10;
    [SerializeField] private float speed = 10;
    [SerializeField] private Material mat;
    [SerializeField, ReadOnly] private GameObject obj;
    

    public string CheckColor(float index, bool colorRed)
    {
        float offset = mat.mainTextureOffset.x + index;
        if (offset % 0.25f < 0.1f && offset % 0.25f > 0 || offset % 0.25f > -0.1f && offset % 0.25f < 0)
        {
            return "fail : border";
        }
        else if (((int)(offset / 0.5f)) % 2 == 0)
        {
            return (!colorRed) ? "good" : "fail";
        }
        else
        {
            return (colorRed) ? "good" : "fail";
        }
    }

    private void SwipeL()
    {
        mat.mainTextureOffset = new Vector2(mat.mainTextureOffset.x + Time.deltaTime * speed, 0);
        // Debug.Log(CheckColor(- 0.25f) + " " +
        //           CheckColor(0) + " " +
        //           CheckColor(+ 0.25f) + " " +
        //           CheckColor(+ 0.5f) + " " +
        //           CheckColor(+ 0.75f) + " " +
        //           CheckColor(+ 1f));
    }

    private void SwipeR()
    {
        mat.mainTextureOffset = new Vector2(- Time.deltaTime * speed, 0);
        // Debug.Log(CheckColor(- 0.25f) + " " +
        //           CheckColor(0) + " " +
        //           CheckColor(+ 0.25f) + " " +
        //           CheckColor(+ 0.5f));
    }

    public void SetMesh(GameObject _obj, int _nbSides)
    {
        obj = _obj;
        nbSide = _nbSides;
        mat.mainTextureOffset = new Vector2(0, 0);
    }

    private void ClickLeft(InteractionKey.InteractionColor color)
    {
        GameManager.onUpdated += SwipeL;
    }

    private void ReleaseLeft()
    {
        GameManager.onUpdated -= SwipeL;
    }

    private void ClickRight(InteractionKey.InteractionColor color)
    {
        GameManager.onUpdated += SwipeR;
    }

    private void ReleaseRight()
    {
        GameManager.onUpdated -= SwipeR;
    }

    private void Start()
    {
        btnLeft.GetComponent<InputListener>().onInputPressed += ClickLeft;
        btnLeft.GetComponent<InputListener>().onInputReleased += ReleaseLeft;
        btnRight.GetComponent<InputListener>().onInputPressed += ClickRight;
        btnRight.GetComponent<InputListener>().onInputReleased += ReleaseRight;
    }

    private void Awake()
    {
        instance = this;
    }
}