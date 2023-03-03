using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerCircle : MonoBehaviour
{
    public byte type; // 0 = Red, 1 = green, 2 = blue
    
    public RectTransform circle;
    public Image bulle;
    
    private float time = 0f;
    private float rate;
    private float maxSizeCircle = 3f;
    private RectTransform rtBulle = null; 
    private InteractionComponent interactionComponent;

    private void Start()
    {
        this.enabled = false;
    }

    private void OnEnable()
    {
        circle.localScale = Vector3.zero;
    }

    public void InitCircle()
    {
        if (!rtBulle) rtBulle = bulle.GetComponent<RectTransform>();
        
        interactionComponent = GetComponent<InteractionComponent>();

        time = 0;
        circle.localScale = new Vector3(maxSizeCircle, maxSizeCircle, 0f);
        //rate =  1 + (interactionComponent.speed - maxSizeCircle) / 10f;
        rate =  interactionComponent.speed * maxSizeCircle;
        // type = (byte)UnityEngine.Random.Range(0, 3);
        // bulle.color = (type == 0) ? Color.red : (type == 1) ? Color.green : Color.blue;
        StartCoroutine(DecreaseCircle());
    }
    

    IEnumerator DecreaseCircle()
    {
        time = Time.deltaTime * rate;

        circle.localScale = new Vector3(circle.localScale.x - time, circle.localScale.y - time, 0f);
        yield return new WaitForEndOfFrame();
        
        if (circle.localScale.x > 0)
        {
            StartCoroutine(DecreaseCircle());
        }
        else
        {
            PatternPoolManager.Instance.AddCircleToPool(gameObject.transform.parent.gameObject);
            InputManager.FailedInteraction();
        }
    }
    
    public float TouchCircle()
    {
        time = 0f;
        PatternPoolManager.Instance.AddCircleToPool(gameObject.transform.parent.gameObject);
        return Mathf.Clamp(
            100 - (circle.GetComponent<RectTransform>().localScale.x - 1) * 100 / (maxSizeCircle - 1), 
            1f, 99.99f);
    }
    
    public void ResizeBulle(float size)
    {
        bulle.rectTransform.localScale = new Vector2(size, size);
    }

   
}
