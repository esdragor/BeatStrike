using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScreenListener : MonoBehaviour,  IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
{
    public RectTransform leftDetector;
    public RectTransform rightDetector;
    private float pressTime;

    private Vector2 onTouchPosition;
    private Vector2 currentTouchPosition;
    
    [Tooltip("A partir de quel distance le swipe se déclenche.")] public float swipeTolerance = 0.2f;
    
    public Action<TouchSide> onInputPressed;
    public Action onInputReleased;
    public Action<SwipeDirection> onSwipeDetected;

    public void OnPointerDown(PointerEventData eventData)
    {
        onTouchPosition = eventData.position;
        onInputPressed?.Invoke(CheckTouchSide());
        GameManager.onUpdated += TouchTime;
    }

    TouchSide CheckTouchSide()
    {
        Bounds leftBounds = new Bounds(leftDetector.transform.position, new Vector3(leftDetector.rect.width, leftDetector.rect.height, 0.0f));
        Bounds rightBounds = new Bounds(rightDetector.transform.position, new Vector3(rightDetector.rect.width, leftDetector.rect.height, 0.0f));
        
        if (leftBounds.Contains(onTouchPosition))
        {
            return TouchSide.LEFT;
        }

        if (rightBounds.Contains(onTouchPosition))
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

