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
    [SerializeField] private float nbColorOnSpine = 16;
    [SerializeField, ReadOnly] private GameObject obj;

    private PlayerInputs inputs;
    private Vector2 startMousePos;

    public void CheckColor(float index, int color)
    {
        float offset = mat.mainTextureOffset.x + index;
        float indexReference = 0.04f * nbColorOnSpine;
        if (offset % color * indexReference < 0.1f && offset % color * indexReference > 0 || 
            offset % color * indexReference > -0.1f && color % indexReference < 0)
        {
            UIManager.instance.announcer.Announce("Fail", Color.white);
            return;
        }
        UIManager.instance.announcer.Announce("Success", Color.white);
    }

    private void SwipeL()
    {
        mat.mainTextureOffset = new Vector2(mat.mainTextureOffset.x + Time.deltaTime * speed, 0f);
    }
    
    private void SwipeR()
    {
        mat.mainTextureOffset = new Vector2(mat.mainTextureOffset.x - Time.deltaTime * speed, 0f);
    }

    public void SetMesh(GameObject _obj, int _nbSides)
    {
        obj = _obj;
        nbSide = _nbSides;
        mat.mainTextureOffset = new Vector2(0, 0);
    }

    private void UpdateWheel()
    {
        Vector2 currentPos = Input.mousePosition;
        currentPos = Input.GetTouch(0).position;
        
        if (Vector2.Distance(startMousePos, currentPos) > 5f)
        {
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
        startMousePos = Input.GetTouch(0).position;
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