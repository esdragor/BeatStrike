using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private static DataManager instance;
    
    public Gear[] saveGearsArray;
    
    
    private void Awake()
    {
        if (!instance) instance = this;
        else Destroy(this);
    }
    // public static void Save()
    // {
    //     DataSerializer.instance.SaveDataOnMainDirectory(GridManager.Instance.gridCells);
    // }
    //
    // public static void Load()
    // {
    //     GridManager.Instance.LoadGrid(saveCellsArray);
    //     saveCellsArray = DataSerializer.instance.LoadDataFromDirectory<Cells[]>();
    // }
}
