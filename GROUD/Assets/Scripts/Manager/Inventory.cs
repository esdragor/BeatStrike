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

        instance.LoadEquipment(instance.LoadInventory());
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

    private async void LoadEquipment(List<GearDescription> gears)
    {
        await Task.Delay(200);
        Gear newGear = null;
        GearDescription gd = null;
        if (DataSerializer.instance.CheckFileExists("Weapon"))
        {
            newGear = DataSerializer.instance.LoadDataFromDirectory<Gear>("Weapon");
            instance.inventoryIDs.Add(DataSerializer.instance.LoadDataFromDirectory<Gear>("Weapon"));
            gd = UI_Gear.AddItemUIInventory(newGear);
            gd.OnClick();
            UI_Gear.Equip();
        }

        if (DataSerializer.instance.CheckFileExists("Chest"))
        {
            newGear = DataSerializer.instance.LoadDataFromDirectory<Gear>("Chest");
            instance.inventoryIDs.Add(DataSerializer.instance.LoadDataFromDirectory<Gear>("Chest"));
            gd = UI_Gear.AddItemUIInventory(newGear);
            gd.OnClick();
            UI_Gear.Equip();
        }

        if (!DataSerializer.instance.CheckFileExists("Head")) return;


        newGear = DataSerializer.instance.LoadDataFromDirectory<Gear>("Head");
        instance.inventoryIDs.Add(DataSerializer.instance.LoadDataFromDirectory<Gear>("Head"));
        gd = UI_Gear.AddItemUIInventory(newGear);
        gd.OnClick();
        UI_Gear.Equip();
    }

    private List<GearDescription> LoadInventory()
    {
        List<GearDescription> gears = new List<GearDescription>();

        if (DataSerializer.instance.CheckFileExists("Inventory"))
        {
            Gear[] inventory = DataSerializer.instance.LoadDataFromDirectory<Gear[]>("Inventory");

            Gear newGear = null;
            GearDescription gd = null;
            for (int i = 0; i < inventory.Length; i++)
            {
                gears.Add(UI_Gear.AddItemUIInventory(inventory[i]));
                instance.inventoryIDs.Add(inventory[i]);
            }
        }

        return gears;
    }

    private void OnDestroy()
    {
        if (OnReset) return;
        string inventory = "";

        if (inventoryIDs.Count > 0)
        {
            DataSerializer.instance.SaveDataOnMainDirectory(inventoryIDs.ToArray(), "Inventory");
        }
        else if (DataSerializer.instance.CheckFileExists("Inventory"))
        {
            DataSerializer.instance.DeleteFile("Inventory");
        }
    }
}