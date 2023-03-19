using System;
using UnityEngine;

public abstract class InteractionComponent : MonoBehaviour
{
    public GameObject target;
    public InteractionKey data;
    public float tolerance = 20f;
    public float speed = 3f;
    public Vector3 startPosition;
    public float scale;
    
    public void SetData(InteractionKey interactionKey)
    {
        data = interactionKey;
        InitializeData();
    }

    private void Update()
    {
        transform.position += -transform.forward * speed * Time.deltaTime;
    }

    public virtual void InitializeData()
    {
        tolerance = data.tolerance;
        //speed = data.drawSpeed;
        startPosition = data.spawnPosition;
        scale = data.scale;
    }

    public virtual void ValidateInteraction()
    {
        if (GameManager.instance.gameState.IsLevelExploration())
        {
            PlayerManager.instance.AddExperience(10f);
        }
        else
        {
           BossManager.instance.AddDamageToPool(10f);
        }

        PatternPoolManager.Instance.AddCircleToPool(gameObject);
    }

    public virtual void HurtPlayer()
    {
        PlayerManager.instance.TakeDamage(10f);
        
        PatternPoolManager.Instance.AddCircleToPool(gameObject);
    }
}