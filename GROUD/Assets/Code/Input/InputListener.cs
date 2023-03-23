using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputListener : MonoBehaviour,  IPointerDownHandler, IPointerUpHandler
{
    public bool isPressed;
    public float touchTime;
    public Action<InteractionKey.InteractionColor> onInputPressed;
    public Action onInputReleased;
    public InteractionKey.InteractionColor listenerColor;

    public void OnPointerUp(PointerEventData eventData)
    {
        InputReleased();
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        InputPressed();
    }
    
    void InputPressed()
    {
        GameManager.onUpdated += TouchTimer;
        isPressed = true;
        
        onInputPressed?.Invoke(listenerColor);
    }

    void InputReleased()
    {
        GameManager.onUpdated -= TouchTimer;
        isPressed = false;
        touchTime = 0;
        
        onInputReleased?.Invoke();
    }

    void TouchTimer()
    {
        touchTime += Time.deltaTime;
    }
}
