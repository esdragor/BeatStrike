using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputListener : MonoBehaviour,  IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
{
    
    public Vector2 onTouchPosition;
    public Vector2 currentTouchPosition;

    public float pressTime;
    public float swipePressDuration = 1.2f;
    public float swipeTolerance = 0.2f;

    public SwipeDirection lastSwipeDirection;

    public RectTransform leftDetector;
    public RectTransform rightDetector;
    
    public Action<TouchSide> onInputPressed;
    public Action onInputReleased;
    public Action<SwipeDirection> onSwipeDetected;
    public InteractionKey.InteractionColor listenerColor;

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Touch");
        onTouchPosition = eventData.position;
        onInputPressed?.Invoke(CheckTouchSide());
        GameManager.onUpdated += TouchTime;
    }

    TouchSide CheckTouchSide()
    {
        if (onTouchPosition.x > leftDetector.position.x && onTouchPosition.x < leftDetector.position.x + leftDetector.rect.width)
        {
            return TouchSide.LEFT;
        }
        
        if (onTouchPosition.x > rightDetector.position.x && onTouchPosition.x < rightDetector.position.x + rightDetector.rect.width)
        {
            return TouchSide.RIGHT;
        }

        return TouchSide.NULL;
    }

    void TouchTime()
    {
        pressTime += Time.deltaTime;
    }
    
    public void OnPointerMove(PointerEventData eventData)
    {
        currentTouchPosition = eventData.position;
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        GameManager.onUpdated -= TouchTime;

        if (pressTime > swipePressDuration)
        {
            CheckForSwipe();
        }

        pressTime = 0;
    }

    void CheckForSwipe()
    {
        Vector2 direction = currentTouchPosition - onTouchPosition;
        Vector2 nDirection = direction.normalized;
        
        if (Mathf.Abs(nDirection.x) > Mathf.Abs(nDirection.y))
        {
            lastSwipeDirection = nDirection.x > 0 ? SwipeDirection.RIGHT : SwipeDirection.LEFT;
        }
        else
        {
            lastSwipeDirection = nDirection.y > 0 ? SwipeDirection.UP : SwipeDirection.DOWN;
        }
        
        onSwipeDetected?.Invoke(lastSwipeDirection);
        
        UIManager.instance.announcer.Announce($"{lastSwipeDirection}", Color.gray);
    }
    
    public enum SwipeDirection
    {
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

