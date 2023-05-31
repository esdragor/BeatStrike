using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolArrowPrinter : MonoBehaviour
{
    private static PoolArrowPrinter instance;

    [SerializeField] private int nbArrowOnPool = 4;
    [SerializeField] private GameObject ArrowUpPrefab;
    [SerializeField] private GameObject ArrowDownPrefab;
    [SerializeField] private GameObject ArrowLeftPrefab;
    [SerializeField] private GameObject ArrowRightPrefab;
    [SerializeField] private int zRotationForArrow;

    private List<GameObject> arrowsUpList = new List<GameObject>();
    private List<GameObject> arrowsDownList = new List<GameObject>();
    private List<GameObject> arrowsLeftList = new List<GameObject>();
    private List<GameObject> arrowsRightList = new List<GameObject>();

    private void Awake()
    {
        if (instance == null) instance = this;
        else
        {
            Destroy(instance.gameObject);
            instance = this;
        }
    }

    private void Start()
    {
        for (int i = 0; i < nbArrowOnPool; i++)
        {
            arrowsUpList.Add(Instantiate(ArrowUpPrefab, transform.position, Quaternion.Euler(0f, -90f+5.9f, zRotationForArrow), transform));
            arrowsDownList.Add(Instantiate(ArrowDownPrefab, transform.position, Quaternion.Euler(0f, 90f+5.9f, -zRotationForArrow), transform));
            arrowsLeftList.Add(Instantiate(ArrowLeftPrefab, transform.position, Quaternion.Euler(zRotationForArrow, -180f+5.9f, 0f), transform));
            arrowsRightList.Add(Instantiate(ArrowRightPrefab, transform.position, Quaternion.Euler(-zRotationForArrow, Quaternion.identity.y+5.9f, Quaternion.identity.z), transform));
        }
    }

    public static GameObject GetArrowOnPool(ScreenListener.SwipeDirection type)
    {
        GameObject arrow = null;
        switch (type)
        {
            case ScreenListener.SwipeDirection.UP:
                arrow = instance.arrowsUpList[0];
                instance.arrowsUpList.RemoveAt(0);
                break;
            case ScreenListener.SwipeDirection.DOWN:
                arrow = instance.arrowsDownList[0];
                instance.arrowsDownList.RemoveAt(0);
                break;
            case ScreenListener.SwipeDirection.LEFT:
                arrow = instance.arrowsLeftList[0];
                instance.arrowsLeftList.RemoveAt(0);
                break;
            case ScreenListener.SwipeDirection.RIGHT:
                arrow = instance.arrowsRightList[0];
                instance.arrowsRightList.RemoveAt(0);
                break;
        }
        arrow.SetActive(true);
        return arrow;
    }

    public static void AddArrowOnPool(GameObject arrow, ScreenListener.SwipeDirection type)
    {
        arrow.SetActive(false);
        switch (type)
        {
            case ScreenListener.SwipeDirection.UP:
                instance.arrowsUpList.Add(arrow);
                break;
            case ScreenListener.SwipeDirection.DOWN:
                instance.arrowsDownList.Add(arrow);
                break;
            case ScreenListener.SwipeDirection.LEFT:
                instance.arrowsLeftList.Add(arrow);
                break;
            case ScreenListener.SwipeDirection.RIGHT:
                instance.arrowsRightList.Add(arrow);
                break;
        }
    }
}