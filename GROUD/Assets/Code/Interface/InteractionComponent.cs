using UnityEngine;

public abstract class InteractionComponent : MonoBehaviour
{
    public GameObject target;
    public InteractionKey data;
    public float tolerance = 20f;
    public float speed = 0.1f;
    public Vector3 startPosition;
    public float scale;

    public abstract void OnInputSuccess();

    public void SetData(InteractionKey interactionKey)
    {
        data = interactionKey;
        InitializeData();
    }
    
    public virtual void InitializeData()
    {
        tolerance = data.tolerance;
        //speed = data.drawSpeed;
        startPosition = data.spawnPosition;
        scale = data.scale;
    }

    public abstract void StartInteraction();
}