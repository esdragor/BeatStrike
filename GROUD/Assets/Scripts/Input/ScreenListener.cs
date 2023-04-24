using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScreenListener : MonoBehaviour,  IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
{
    private float pressTime;

    private Vector2 onTouchPosition;
    private Vector2 currentTouchPosition;
    
    [Tooltip("A partir de quel distance le swipe se déclenche.")] public float swipeTolerance = 0.2f;
    
    public Action<TouchSide> onInputPressed;
    public Action onInputReleased;
    public Action<SwipeDirection> onSwipeDetected;

    private float screenCenter;

    private void Awake()
    {
        screenCenter = Screen.width * 0.5f;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        onTouchPosition = eventData.position;
        onInputPressed?.Invoke(CheckTouchSide());
        GameManager.onUpdated += TouchTime;
    }

    TouchSide CheckTouchSide()
    {
        if (GameManager.instance.gameState.IsEngineMenu()) return TouchSide.NULL;
     
        if (onTouchPosition.x < screenCenter)
        {
            return TouchSide.LEFT;
        }

        if (onTouchPosition.x > screenCenter)
        {
            return TouchSide.RIGHT;
        }

        return TouchSide.NULL;
    }

    void TouchTime()
    {
        pressTime += Time.deltaTime;
        
        float mag = Mathf.Abs(currentTouchPosition.magnitude - onTouchPosition.magnitude);

        if (mag > swipeTolerance && !swiping)
        {
            onSwipeDetected?.Invoke(CheckForSwipe());
        }
    }
    
    public void OnPointerMove(PointerEventData eventData)
    {
        currentTouchPosition = eventData.position;
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        GameManager.onUpdated -= TouchTime;
        swiping = false;
        pressTime = 0;
        
        onInputReleased?.Invoke();
    }

    private bool swiping;
    SwipeDirection CheckForSwipe()
    {
        swiping = true;

        Vector2 direction = currentTouchPosition - onTouchPosition;
        Vector2 nDirection = direction.normalized;
        if (Mathf.Abs(nDirection.x) > Mathf.Abs(nDirection.y))
        {
            switch (nDirection.x)
            {
                case > 0:
                    return SwipeDirection.RIGHT;
                case < 0:
                    return SwipeDirection.LEFT;
            }
        }
        else
        {
            switch (nDirection.y)
            {
                case > 0:
                    return SwipeDirection.UP;
                case < 0:
                    return SwipeDirection.DOWN;
            }
        }

        return SwipeDirection.NULL;
    }
    
    public enum SwipeDirection
    {
        NULL,
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    public enum TouchSide
    {
        NULL,
        LEFT,
        RIGHT
    }


}

