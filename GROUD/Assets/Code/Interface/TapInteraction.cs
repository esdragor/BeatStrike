using System;
using UnityEngine;

public class TapInteraction : InteractionComponent
{
    public RectTransform objectPosition;
    private Transform bulleTr;
    private TimerCircle timerCircle;
    private Camera cam;

    private void Start()
    {
        objectPosition.anchoredPosition = startPosition;
        cam = Camera.main;
        
    }

    private void OnSimpleTouch(Vector2 pos)
    {
        Vector3 newPos2 = cam.WorldToScreenPoint(bulleTr.position);
        float newTolerance = tolerance  + GameManager.instance.DistanceBetweenPoints;
        
        if (newPos2.x - newTolerance  < pos.x &&
            newPos2.x + newTolerance  > pos.x &&
            newPos2.y - newTolerance  < pos.y &&
            newPos2.y + newTolerance  > pos.y)
        {
            GameManager.instance.AddSuccessTouch(timerCircle.TouchCircle());
        }
    }

    public override void OnInputSuccess()
    {
    }

    public override void StartInteraction()
    {
        timerCircle.InitCircle();
    }

    public override void ActivateInteraction()
    {
        timerCircle = GetComponent<TimerCircle>();
        InputManager.OnSimpleTouch += OnSimpleTouch;
        bulleTr = timerCircle.transform.parent;
        objectPosition.anchoredPosition = startPosition;
    }

    private void OnDisable()
    {
        InputManager.OnSimpleTouch -= OnSimpleTouch;
    }
}