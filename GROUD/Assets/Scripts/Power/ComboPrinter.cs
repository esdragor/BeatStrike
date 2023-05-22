using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboPrinter : MonoBehaviour
{
    private static ComboPrinter instance;

    [SerializeField] private Transform[] ArrowSpawnPoint;
    [SerializeField] private MeshRenderer meterRenderer;
    private List<ArrowInfo> arrows = new List<ArrowInfo>();

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
        PoolArrowPrinter.AddArrowOnPool(instance.arrows[0].arrow, instance.arrows[0].direction);
        instance.arrows.RemoveAt(0);
        for (int i = 0; i < instance.arrows.Count; i++)
        {
            instance.arrows[i].arrow.transform.position = instance.ArrowSpawnPoint[i].position;
        }
        PlayerManager.instance.vfxManager.PlaySFX("GCombo");
    }

    public static void UpdateMeter(float currentCombo)
    {
        instance.meterRenderer.material.SetFloat("_AbilityMeter", currentCombo / (instance.arrows.Capacity - 1));
    }

    public static void PrintNewCombo(ScreenListener.SwipeDirection[] combo)
    {
        foreach (var arrow in instance.arrows)
        {
            PoolArrowPrinter.AddArrowOnPool(arrow.arrow, arrow.direction);
        }
        instance.arrows.Clear();
        
        for (int i = 0; i < combo.Length; i++)
        {
            GameObject arrow = null;
            ScreenListener.SwipeDirection direction = ScreenListener.SwipeDirection.UP;
            switch (combo[i])
            {
                case ScreenListener.SwipeDirection.UP:
                    arrow = PoolArrowPrinter.GetArrowOnPool(ScreenListener.SwipeDirection.UP);
                    break;
                case ScreenListener.SwipeDirection.DOWN:
                    arrow = PoolArrowPrinter.GetArrowOnPool(ScreenListener.SwipeDirection.DOWN);
                    direction = ScreenListener.SwipeDirection.DOWN;
                    break;
                case ScreenListener.SwipeDirection.LEFT:
                    arrow = PoolArrowPrinter.GetArrowOnPool(ScreenListener.SwipeDirection.LEFT);
                    direction = ScreenListener.SwipeDirection.LEFT;
                    break;
                case ScreenListener.SwipeDirection.RIGHT:
                    arrow = PoolArrowPrinter.GetArrowOnPool(ScreenListener.SwipeDirection.RIGHT);
                    direction = ScreenListener.SwipeDirection.RIGHT;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            instance.arrows.Add(new ArrowInfo(arrow, direction));
            arrow.transform.position = instance.ArrowSpawnPoint[i].position;
        }
    }
}

public struct ArrowInfo
{
    public GameObject arrow;
    public ScreenListener.SwipeDirection direction;

    public ArrowInfo(GameObject arrow, ScreenListener.SwipeDirection direction)
    {
        this.arrow = arrow;
        this.direction = direction;
    }
}