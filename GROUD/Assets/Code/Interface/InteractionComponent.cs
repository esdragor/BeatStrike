using UnityEngine;
using Utilities;

public class InteractionComponent : MonoBehaviour
{
    public InteractionKey data;
    public float speed = 3f;
    public InteractionSuccess successGroup;

    private void Update()
    {
        transform.position += -transform.forward * speed * Time.deltaTime;
    }
    
    public void SetData(InteractionKey interactionKey)
    {
        data = interactionKey;
        successGroup = InteractionSuccess.Perfect;
        SetVisualAndColor();
    }

    private MeshRenderer renderer => GetComponent<MeshRenderer>();
    public void SetSuccess(InteractionSuccess itSuccess)
    {
        successGroup = itSuccess;
    }

    public void SetVisualAndColor()
    {
        switch (data.interactionType)
        {
            case Enums.InteractionType.Tap:
                transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                
                switch (data.interactionColor)
                {
                    case InteractionKey.InteractionColor.Blue:
                        renderer.material.color = Color.blue;
                        break;
            
                    case InteractionKey.InteractionColor.Red:
                        renderer.material.color = Color.red;
                        break;
                }
                
                break;
            
            case Enums.InteractionType.Swipe:
                transform.localScale = new Vector3(1f, 0.5f, 0.5f);
                
                renderer.material.color = Color.green;
                
                break;
        }
    }

    public virtual void ValidateInteraction()
    {
        
        if (GameManager.instance.gameState.IsLevelExploration())
        {
            PlayerManager.instance.AddExperience(10f);
        }
        else
        {
           BossManager.instance.AddDamageToPool(PlayerManager.instance.currentStats.damage);
        }
        
        PlayerManager.instance.OnInteractionSuccess(successGroup);
        
        PatternPoolManager.Instance.AddCircleToPool(gameObject);
    }

    public virtual void HurtPlayer()
    {
        PlayerManager.instance.TakeDamage(10f);
        
        PatternPoolManager.Instance.AddCircleToPool(gameObject);
    }
}