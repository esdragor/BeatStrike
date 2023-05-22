using System.Collections.Generic;
using UnityEngine;

public class InteractionPool
{
    public List<GameObject> activeInteractions = new ();
    
    private GameObject interactionPrefab;
    private int poolSize = 10;
    private Transform interactionParent;
    
    private List<GameObject> interactionPool = new ();

    public InteractionPool(Transform interactionParent, GameObject prefab)
    {
        this.interactionParent = interactionParent;
        this.interactionPrefab = prefab;

        InitInteractionPool();
    }
    
    public void InitInteractionPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            interactionPool.Add(Object.Instantiate(interactionPrefab, interactionParent));
            interactionPool[i].name = $"Interaction_{i}";
            interactionPool[i].transform.parent.SetParent(interactionParent);
            interactionPool[i].SetActive(false);
        }
    }

    public void DisableAllInteractions()
    {
        foreach (GameObject it in activeInteractions)
        {
            it.SetActive(false);
            //activeInteractions.Remove(it);
            it.transform.parent = interactionParent;
            if (!interactionPool.Contains(it))
            {
                interactionPool.Add(it);
            }
        }
        
        activeInteractions.Clear();
    }

    public void AddInteractionToPool(GameObject it)
    {
        it.SetActive(false);
        
        activeInteractions.Remove(it);
        it.transform.parent = interactionParent;
        
        if (!interactionPool.Contains(it))
        {
            interactionPool.Add(it);
        }
    }

    public List<GameObject> GetInteractionPool()
    {
        return activeInteractions;
    }

    public GameObject GetCircleFromPool()
    {
        if (interactionPool.Count > 0)
        {
            var interaction = interactionPool[0];
            interactionPool.RemoveAt(0);
            
            interaction.SetActive(true);
            activeInteractions.Add(interaction);
            
            return interaction;
        }
        else
        {
            var interaction = Object.Instantiate(interactionPrefab, interactionParent);
            
            interaction.SetActive(true);
            interaction.transform.SetParent(interactionParent);
            activeInteractions.Add(interaction);
            
            return interaction;
        }
    }
}
