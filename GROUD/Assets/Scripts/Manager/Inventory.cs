using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private static Inventory instance;


    [SerializeField] private Gear[] gearsDatas;

    private List<Gear> inventoryIDs = new List<Gear>();
    private Dictionary<int, Gear> dicoGear = new Dictionary<int, Gear>();
    private bool OnReset = false;
    private int ID = -1;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public static void Init()
    {
        foreach (var gear in instance.gearsDatas)
        {
            instance.dicoGear.Add(gear.ID, gear);
        }

        instance.LoadInventory();
        instance.LoadEquipment();
    }

    public static Gear[] GetGearsData()
    {
        return instance.gearsDatas;
    }

    public static GearDescription AddItemOnInventory(Gear gear)
    {
        instance.inventoryIDs.Add(gear);
        return UI_Gear.AddItemUIInventory(gear);
    }

    public static void RemoveItemOnInventory(Gear gear)
    {
        instance.inventoryIDs.Remove(gear);
        UI_Gear.RemoveItemUIInventory(gear);
    }

    public static void OnResetValue()
    {
        instance.OnReset = true;
    }

    private async void LoadEquipment()
    {
        await Task.Delay(200);
        Gear newGear = null;
        GearDescription gd = null;
        if (DataSerializer.instance.CheckFileExists("Weapon"))
        {
            newGear = DataSerializer.instance.LoadDataFromDirectory<Gear>("Weapon");
            newGear.ID = GetNewIndexID();
            //instance.inventoryIDs.Add(DataSerializer.instance.LoadDataFromDirectory<Gear>("Weapon"));
            gd = UI_Gear.AddItemUIInventory(newGear);
            gd.OnClick();
            UI_Gear.Equip();
        }

        if (DataSerializer.instance.CheckFileExists("Chest"))
        {
            newGear = DataSerializer.instance.LoadDataFromDirectory<Gear>("Chest");
            newGear.ID = GetNewIndexID();
            //instance.inventoryIDs.Add(DataSerializer.instance.LoadDataFromDirectory<Gear>("Chest"));
            gd = UI_Gear.AddItemUIInventory(newGear);
            gd.OnClick();
            UI_Gear.Equip();
        }

        if (!DataSerializer.instance.CheckFileExists("Head")) return;


        newGear = DataSerializer.instance.LoadDataFromDirectory<Gear>("Head");
        newGear.ID = GetNewIndexID();
        //instance.inventoryIDs.Add(DataSerializer.instance.LoadDataFromDirectory<Gear>("Head"));
        gd = UI_Gear.AddItemUIInventory(newGear);
        gd.OnClick();
        UI_Gear.Equip();
    }

    private void LoadInventory()
    {
        if (DataSerializer.instance.CheckFileExists("Inventory"))
        {
            Gear[] inventory = DataSerializer.instance.LoadDataFromDirectory<Gear[]>("Inventory");

            for (int i = 0; i < inventory.Length; i++)
            {
                inventory[i].ID = GetNewIndexID();
            }

            Gear newGear = null;
            GearDescription gd = null;
            for (int i = 0; i < inventory.Length; i++)
            {
                instance.inventoryIDs.Add(inventory[i]);
                UI_Gear.AddItemUIInventory(inventory[i]);
            }
        }
    }

    public static void RemoveFromInventory(Gear gear)
    {
        foreach (var _gear in instance.inventoryIDs)
        {
            if (gear.ID != _gear.ID) continue;
            instance.inventoryIDs.Remove(_gear);
            break;
        }
    }

    public static void AddToInventory(Gear gear)
    {
        instance.inventoryIDs.Add(gear);
    }

    private void OnDestroy()
    {
        if (OnReset) return;

        if (inventoryIDs.Count > 0)
        {
            DataSerializer.instance.SaveDataOnMainDirectory(inventoryIDs.ToArray(), "Inventory");
        }
        else if (DataSerializer.instance.CheckFileExists("Inventory"))
        {
            DataSerializer.instance.DeleteFile("Inventory");
        }
    }

    public static int GetNewIndexID()
    {
        instance.ID++;
        return instance.ID;
    }
}