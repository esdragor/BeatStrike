using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerCircle : MonoBehaviour
{
    public byte type; // 0 = Red, 1 = green, 2 = blue
    
    [SerializeField] private RectTransform Circle;
    [SerializeField] private Image Bulle;
    
    private float time = 0f;
    private float rate;
    private float maxSizeCircle = 3f;
    private RectTransform rtBulle = null; 
    private PatternComponent patternComponent;

    private void Start()
    {
        patternComponent = GetComponent<PatternComponent>();
    }

    IEnumerator DecreaseCircle()
    {
        time -= Time.deltaTime * rate * maxSizeCircle;
        if (!rtBulle)
            rtBulle = Bulle.GetComponent<RectTransform>();
        Circle.localScale = new Vector3(rtBulle.localScale.x +time, rtBulle.localScale.y + time, 0f);
        yield return new WaitForEndOfFrame();
        if (time > 0f)
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
            100 - (Circle.GetComponent<RectTransform>().localScale.x - 1) * 100 / (maxSizeCircle - 1), 
            1f, 99.99f);
    }
    
    public void ResizeBulle(float size)
    {
        Bulle.rectTransform.localScale = new Vector2(size, size);
    }
    
    void OnEnable()
    {
        time = maxSizeCircle;
        rate = patternComponent.speed;
        type = (byte) UnityEngine.Random.Range(0, 3);
        Bulle.color = (type == 0) ? Color.red : (type == 1) ? Color.green : Color.blue;
        StartCoroutine(DecreaseCircle());
    }
    
}
