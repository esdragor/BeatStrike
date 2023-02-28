using UnityEngine;
using UnityEngine.UI;

public class TapPattern : PatternComponent
{
    private RectTransform bulleTr;
    private void Start()
    {
        bulleTr = GetComponent<RectTransform>();
    }
    
    private void OnEnable()
    {
        InputManager.OnSimpleTouch += OnSimpleTouch;
    }

    private void OnDisable()
    {
        InputManager.OnSimpleTouch -= OnSimpleTouch;
    }
    
    private void OnSimpleTouch(Vector2 pos)
    {
        if (bulleTr.position.x - tolerance - bulleTr.rect.width < pos.x &&
            bulleTr.position.x + tolerance + bulleTr.rect.width > pos.x &&
            bulleTr.position.y - tolerance - bulleTr.rect.width < pos.y &&
            bulleTr.position.y + tolerance + bulleTr.rect.width > pos.y)
        {
            Debug.Log("Touch");
        }
        else
        {
            Debug.Log("Pas Touch");
        }
    }

    public override void OnInputSuccess()
    {
        
    }
}
