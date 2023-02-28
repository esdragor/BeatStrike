using System.Collections.Generic;
using UnityEngine;

public class PatternPoolManager : MonoBehaviour
{
    public static PatternPoolManager Instance;
    public List<GameObject> ActiveCircles = new ();
    [HideInInspector] public float CircleWidth = 50f;

    [SerializeField] private GameObject circlePrefab;
    [SerializeField] private int maxPoolSize = 10;
    
    private List<GameObject> circlePool = new ();

    private void Awake()
    {
        Instance = this;
        InitCirclePool();
    }
    
    void InitCirclePool()
    {
        for (int i = 0; i < maxPoolSize; i++)
        {
            circlePool.Add(Instantiate(circlePrefab, transform));
            circlePool[i].SetActive(false);
        }
        CircleWidth = circlePrefab.transform.GetChild(0).GetComponent<RectTransform>().rect.width;
    }

    public void AddCircleToPool(GameObject circle)
    {
        circle.SetActive(false);
        
        ActiveCircles.Remove(circle);
        circle.transform.parent = transform;
        circlePool.Add(circle);
    }
    
    public GameObject GetCircleFromPool()
    {
        if (circlePool.Count > 0)
        {
            var circle = circlePool[0];
            circlePool.RemoveAt(0);
            circle.SetActive(true);
            ActiveCircles.Add(circle);
            return circle;
        }
        else
        {
            var circle = Instantiate(circlePrefab, transform);
            circle.SetActive(true);
            ActiveCircles.Add(circle);
            return circle;
        }
    }
}
