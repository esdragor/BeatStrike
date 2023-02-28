using System;
using UnityEngine;

public class TapInteraction : InteractionComponent
{
    public RectTransform objectPosition;
    private RectTransform bulleTr;
    private TimerCircle timerCircle;
    
    private void OnEnable()
    {
        timerCircle = GetComponent<TimerCircle>();
        InputManager.OnSimpleTouch += OnSimpleTouch;
    }
    
    private void Start()
    {
        bulleTr = timerCircle.circle;
        objectPosition.localScale = new Vector3(scale, scale, scale);
        objectPosition.anchoredPosition = startPosition;
    }

    private void Update()
    {
    }

    private void OnSimpleTouch(Vector2 pos)
    {
        if (bulleTr.position.x - tolerance - bulleTr.localScale.x  * bulleTr.rect.width < pos.x &&
            bulleTr.position.x + tolerance + bulleTr.localScale.x * bulleTr.rect.width > pos.x &&
            bulleTr.position.y - tolerance - bulleTr.localScale.y * bulleTr.rect.height < pos.y &&
            bulleTr.position.y + tolerance + bulleTr.localScale.y * bulleTr.rect.height > pos.y)
        {
            PatternPoolManager.Instance.AddCircleToPool(gameObject);
            Debug.Log("TOUCH !");
        }
    }

    public override void OnInputSuccess()
    {
        
    }

    public override void StartInteraction()
    {
        timerCircle.InitCircle();
    }

    private void OnDisable()
    {
        InputManager.OnSimpleTouch -= OnSimpleTouch;
    }
}
