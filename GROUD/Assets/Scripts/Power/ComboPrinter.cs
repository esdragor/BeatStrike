using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboPrinter : MonoBehaviour
{
    private static ComboPrinter instance;

    [SerializeField] private GameObject ArrowUpPrefab;
    [SerializeField] private GameObject ArrowDownPrefab;
    [SerializeField] private GameObject ArrowLeftPrefab;
    [SerializeField] private GameObject ArrowRightPrefab;

    [SerializeField] private Transform[] ArrowSpawnPoint;
    
    private List<GameObject> arrows = new List<GameObject>();

    private void Awake()
    {
        if (instance == null) instance = this;
        else
        {
            Destroy(instance.gameObject);
            instance = this;
        }
    }

    public static void UpdateCombo()
    {
        Destroy(instance.arrows[0]);
        instance.arrows.RemoveAt(0);
        for (int i = 0; i < instance.arrows.Count; i++)
        {
            instance.arrows[i].transform.position = instance.ArrowSpawnPoint[i].position;
        }
    }

    public static void PrintNewCombo(ScreenListener.SwipeDirection[] combo)
    {
        foreach (var arrow in instance.arrows)
        {
            Destroy(arrow);
        }
        instance.arrows.Clear();
        
        for (int i = 0; i < combo.Length; i++)
        {
            switch (combo[i])
            {
                case ScreenListener.SwipeDirection.UP:
                    instance.arrows.Add(Instantiate(instance.ArrowUpPrefab, instance.ArrowSpawnPoint[i].position, Quaternion.Euler(0f, -90f, 0f)));
                    break;
                case ScreenListener.SwipeDirection.DOWN:
                    instance.arrows.Add(Instantiate(instance.ArrowDownPrefab, instance.ArrowSpawnPoint[i].position, Quaternion.Euler(0f, 90f, 0f)));
                    break;
                case ScreenListener.SwipeDirection.LEFT:
                    instance.arrows.Add(Instantiate(instance.ArrowLeftPrefab, instance.ArrowSpawnPoint[i].position, Quaternion.Euler(0f, -180f, 0f)));
                    break;
                case ScreenListener.SwipeDirection.RIGHT:
                    instance.arrows.Add(Instantiate(instance.ArrowRightPrefab, instance.ArrowSpawnPoint[i].position, Quaternion.identity));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            //instance.arrows[i].transform.parent = instance.ArrowSpawnPoint[i];
        }
    }
}