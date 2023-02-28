using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerCircle : MonoBehaviour
{
    public byte type; // 0 = Red, 1 = green, 2 = blue
    
    [SerializeField] public RectTransform circle;
    [SerializeField] private Image bulle;
    
    private float time = 0f;
    private float rate;
    private float maxSizeCircle = 3f;
    private RectTransform rtBulle = null; 
    private InteractionComponent interactionComponent;
    
    public void InitCircle()
    {
        if (!rtBulle) rtBulle = bulle.GetComponent<RectTransform>();
        
        interactionComponent = GetComponent<InteractionComponent>();

        time = maxSizeCircle;
        rate = interactionComponent.speed;
        type = (byte)UnityEngine.Random.Range(0, 3);
        bulle.color = (type == 0) ? Color.red : (type == 1) ? Color.green : Color.blue;
        StartCoroutine(DecreaseCircle());
    }
    
    IEnumerator DecreaseCircle()
    {
        time -= Time.deltaTime * rate * maxSizeCircle;

        circle.localScale = new Vector3(rtBulle.localScale.x + time, rtBulle.localScale.y + time, 0f);
        
        yield return new WaitForEndOfFrame();
        
        if (circle.localScale.x > 0)
        {
            StartCoroutine(DecreaseCircle());
        }
        else
        {
            PatternPoolManager.Instance.AddCircleToPool(gameObject);
            InputManager.OnFailedTouchInteraction?.Invoke();
        }
    }
    
    public void TouchCircle()
    {
        time = -1f;
        PatternPoolManager.Instance.AddCircleToPool(gameObject);
        float succes = Mathf.Clamp(
            100 - (circle.GetComponent<RectTransform>().localScale.x - 1) * 100 / (maxSizeCircle - 1), 
            1f, 99.99f);
    }
    
    public void ResizeBulle(float size)
    {
        bulle.rectTransform.localScale = new Vector2(size, size);
    }

   
}
