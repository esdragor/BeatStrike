using System;
using System.Collections.Generic;
using UnityEngine;

public class PatternPoolManager : MonoBehaviour
{
    public static PatternPoolManager Instance;
    public List<GameObject> ActiveInteractions = new ();
    [SerializeField] private GameObject interactionPrefab;
    [SerializeField] private int maxPoolSize = 10;
    
    private List<GameObject> interactionPool = new ();

    private void Awake()
    {
        Instance = this;
        InitCirclePool();
    }
    
    void InitCirclePool()
    {
        for (int i = 0; i < maxPoolSize; i++)
        {
            interactionPool.Add(Instantiate(interactionPrefab, transform));
            interactionPool[i].name = $"Interaction_{i}";
            interactionPool[i].SetActive(false);
        }
    }

    public void DisableAllInteractions()
    {
        foreach (GameObject it in ActiveInteractions)
        {
            it.SetActive(false);
            it.transform.parent = transform;
            interactionPool.Add(it);
        }
        
        ActiveInteractions.Clear();
    }

    public void AddInteractionToPool(GameObject it)
    {
        it.SetActive(false);
        
        ActiveInteractions.Remove(it);
        it.transform.parent = transform;
        interactionPool.Add(it);
    }
    
    public GameObject GetCircleFromPool()
    {
        if (interactionPool.Count > 0)
        {
            var circle = interactionPool[0];
            interactionPool.RemoveAt(0);
            circle.SetActive(true);
            ActiveInteractions.Add(circle);
            return circle;
        }
        else
        {
            var circle = Instantiate(interactionPrefab, transform);
            circle.SetActive(true);
            ActiveInteractions.Add(circle);
            return circle;
        }
    }
}
