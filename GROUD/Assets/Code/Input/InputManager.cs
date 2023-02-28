using System;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    private PlayerInputs MyInputManager;
    
    public delegate void StartTouch(Vector2 pos, float time);
    public event StartTouch OnStartEvent;
    public delegate void EndTouch(Vector2 pos, float time);
    public event EndTouch OnEndEvent;
    
    
    [SerializeField] private float minDist = 0.1f;
    [SerializeField] private float MinTimeTouch = 0.05f;
    [SerializeField] private float tolerance = 0.8f;

    private Vector2 StartPosSwipe = Vector2.zero;
    private float StartTimerSwipe = 0f;
    
    public static event Action<Vector2, SwipeDirection> OnSwipeUp;
    public static event Action<Vector2, SwipeDirection> OnSwipeDown;
    public static event Action<Vector2, SwipeDirection> OnSwipeLeft;
    public static event Action<Vector2, SwipeDirection> OnSwipeRight;
    public static Action OnFailedTouchInteraction;
    public static event Action<Vector2> OnSimpleTouch;

    public static Vector3 ScreenToWorld(Camera cam, Vector3 pos)
    {
        pos.z = cam.nearClipPlane;
        return cam.ScreenToWorldPoint(pos);
    }
    
    private void Awake()
    {
        MyInputManager = new PlayerInputs();
        Instance = this;
    }

    private void OnEnable()
    {
        MyInputManager.Enable();
        OnStartEvent += SwipeStart;
        OnEndEvent += SwipeEnd;
    }
    
    private void OnDisable()
    {
        OnStartEvent -= SwipeStart;
        OnEndEvent -= SwipeEnd;
    }

    private void Start()
    {
        MyInputManager.Touch.PrimaryContact.started += ctx => StartTouchP(ctx);
        MyInputManager.Touch.PrimaryContact.canceled += ctx => EndTouchP(ctx);
    }

    private void StartTouchP(InputAction.CallbackContext ctx)
    {
        OnStartEvent(
            /*ScreenToWorld(Camera.main, MyInputManager.Touch.PrimaryPosition.ReadValue<Vector2>())*/MyInputManager.Touch.PrimaryPosition.ReadValue<Vector2>(), 
            (float)ctx.startTime);
    }
    
    private void EndTouchP(InputAction.CallbackContext ctx)
    {
        OnEndEvent(
            /*ScreenToWorld(Camera.main, MyInputManager.Touch.PrimaryPosition.ReadValue<Vector2>())*/MyInputManager.Touch.PrimaryPosition.ReadValue<Vector2>(), 
            (float)ctx.startTime);
    }

    private void SwipeEnd(Vector2 pos, float time)
    {
        // if (Vector3.Distance(StartPosSwipe, pos) >= minDist/* && (time - StartTimerSwipe) > MinTimeTouch*/)
        // {
        //     Vector2 dir2 = (pos - StartPosSwipe).normalized;
        //     SwipeDirection(dir2);
        // }
        // else
        {
            SimpleTouch(pos);
        }
    }

    private void SimpleTouch(Vector2 pos)
    {
        OnSimpleTouch?.Invoke(pos);
    }
    
    private void SwipeDirection(Vector2 direction)
    {
        if (Vector2.Dot(Vector2.up, direction) > tolerance)
        {
            OnSwipeUp?.Invoke(direction, global::SwipeDirection.Up);
        }
        else if (Vector2.Dot(Vector2.down, direction) > tolerance)
        {
            OnSwipeDown?.Invoke(direction, global::SwipeDirection.Down);
        }
        else if (Vector2.Dot(Vector2.left, direction) > tolerance)
        {
            OnSwipeLeft?.Invoke(direction, global::SwipeDirection.Left);
        }
        else if (Vector2.Dot(Vector2.right, direction) > tolerance)
        {
            OnSwipeRight?.Invoke(direction, global::SwipeDirection.Right);
        }
    }

    private void SwipeStart(Vector2 pos, float time)
    {
        StartPosSwipe = pos;
        StartTimerSwipe = time;
    }
}
