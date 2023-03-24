using System;
using UnityEngine;

public abstract class InteractionComponent : MonoBehaviour
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
        SetColor();
    }

    private MeshRenderer renderer => GetComponent<MeshRenderer>();
    public void SetSuccess(InteractionSuccess itSuccess)
    {
        successGroup = itSuccess;
    }

    public void SetColor()
    {
        switch (data.interactionColor)
        {
            case InteractionKey.InteractionColor.Blue:
                renderer.material.color = Color.blue;
                break;
            
            case InteractionKey.InteractionColor.Red:
                renderer.material.color = Color.red;
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
        
        Debug.Log(successGroup);
        PlayerManager.instance.OnInteractionSuccess(successGroup);
        
        PatternPoolManager.Instance.AddCircleToPool(gameObject);
    }

    public virtual void HurtPlayer()
    {
        PlayerManager.instance.TakeDamage(10f);
        
        PatternPoolManager.Instance.AddCircleToPool(gameObject);
    }
}