using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;
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

    private PlayerInputs inputs;
    private Vector2 startMousePos;

    public string CheckColor(float index, int color)
    {
        float offset = mat.mainTextureOffset.x + index;
        if (offset % 0.25f < 0.1f && offset % 0.25f > 0 || offset % 0.25f > -0.1f && offset % 0.25f < 0)
        {
            return "fail : border";
        }
        // else if (((int)(offset / 0.5f)) % 2 == 0)
        // {
        //     return (!color) ? "good" : "fail";
        // }
        // else
        // {
        //     return (color) ? "good" : "fail";
        // }
        return null;
    }

    private void SwipeL()
    {
        mat.mainTextureOffset = new Vector2(mat.mainTextureOffset.x + Time.deltaTime * speed, 0.5f);
    }
    
    private void SwipeR()
    {
        mat.mainTextureOffset = new Vector2(mat.mainTextureOffset.x - Time.deltaTime * speed, 0.5f);
    }

    public void SetMesh(GameObject _obj, int _nbSides)
    {
        obj = _obj;
        nbSide = _nbSides;
        mat.mainTextureOffset = new Vector2(0, 0.5f);
    }

    private void UpdateWheel()
    {
        Vector2 currentPos = Input.mousePosition;
        //Vector2 currentPos = Input.GetTouch(0).position;
        
        if (Vector2.Distance(startMousePos, currentPos) > 5f)
        {
            Debug.Log("Distance : " + Vector2.Distance(startMousePos, currentPos));
            if (startMousePos.x > currentPos.x)
            {
                SwipeL();
            }
            else
            {
                SwipeR();
            }
            
            startMousePos = currentPos;
        }
    }

    private void Click(InputAction.CallbackContext ctx)
    {
        startMousePos = Input.mousePosition;
        //startMousePos = Input.GetTouch(0).position;
        GameManager.onUpdated += UpdateWheel;
    }

    private void Release(InputAction.CallbackContext ctx)
    {
        startMousePos = Vector2.zero;
        GameManager.onUpdated -= UpdateWheel;
    }

    private void Start()
    {
        inputs ??= new PlayerInputs();
        inputs.Enable();
        
        inputs.Touch.PrimaryContact.performed += Click;
        inputs.Touch.PrimaryContact.canceled += Release;
    }

    private void Awake()
    {
        instance = this;
    }
}