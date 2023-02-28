using UnityEngine;
using Utilities;

public enum SwipeDirection
{
    Up,
    Down,
    Left,
    Right
}

public class PatternManager : MonoBehaviour
{
    public static PatternManager Instance;

    public InteractionKey testDataKey;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        DrawInteractionOnScreen(testDataKey);
    }

    public void DrawInteractionOnScreen(InteractionKey dataKey)
    {
        GameObject interactionObj = null;
        InteractionComponent interactionComponent = null;
        
        switch (dataKey.interactionType)
        {
            case Enums.InteractionType.Tap:
                interactionObj = PatternPoolManager.Instance.GetCircleFromPool();
                interactionComponent = interactionObj.GetComponent<InteractionComponent>();
                TapInteraction tapIn = (TapInteraction)interactionComponent;
                tapIn.SetData(dataKey);
                break;
            
            case Enums.InteractionType.Hold:
                break;
            
            case Enums.InteractionType.Slide:
                break;
            
            case Enums.InteractionType.Spam:
                break;
        }

        if (interactionComponent != null) interactionComponent.StartInteraction();
    }
}
