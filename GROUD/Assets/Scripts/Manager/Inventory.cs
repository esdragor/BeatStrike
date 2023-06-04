using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private static Inventory instance;

    [SerializeField] private Sprite spriteWeapon; 
    [SerializeField] private Sprite spriteChest; 
    [SerializeField] private Sprite spriteHead;

    private List<Gear> inventoryIDs = new List<Gear>();
    private bool OnReset = false;
    private int ID = -1;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        Init();
    }

    public static void Init()
    {
        UI_Gear.ClearInventory();
        instance.LoadInventory();
        instance.LoadEquipment();
    }

    public static void AddItemOnInventory(Gear gear)
    {
        instance.inventoryIDs.Add(gear);
        UI_Gear.AddItemUIInventory(gear);
    }

    public static void RemoveItemOnInventory(Gear gear)
    {
        instance.inventoryIDs.Remove(gear);
        UI_Gear.RemoveItemUIInventory(gear);
    }

    public static Sprite GetSprite(GearSlot gear)
    {
        switch (gear)
        {
            case GearSlot.Head:
                return instance.spriteHead;
            case GearSlot.Chest:
                return instance.spriteChest;
            case GearSlot.Weapon:
                return instance.spriteWeapon;
            default:
                throw new ArgumentOutOfRangeException(nameof(gear), gear, null);
        }
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
        GearSaveData gearData;
        if (DataSerializer.instance.CheckFileExists("Weapon"))
        {
            
            gearData = DataSerializer.instance.LoadDataFromDirectory<GearSaveData>("Weapon");
            
            newGear = ScriptableObject.CreateInstance<Gear>();
            newGear.CopyGear(gearData);
            newGear.ID = GetNewIndexID();
            newGear.gearSprite = GetSprite(newGear.slot);
            gd = UI_Gear.AddItemUIInventory(newGear);
            gd.OnClick();
            UI_Gear.Equip();
        }

        if (DataSerializer.instance.CheckFileExists("Chest"))
        {
            gearData = DataSerializer.instance.LoadDataFromDirectory<GearSaveData>("Chest");
            
            newGear = ScriptableObject.CreateInstance<Gear>();
            newGear.CopyGear(gearData);
            newGear.ID = GetNewIndexID();
            newGear.gearSprite =GetSprite(newGear.slot);
            gd = UI_Gear.AddItemUIInventory(newGear);
            gd.OnClick();
            UI_Gear.Equip();
        }

        if (!DataSerializer.instance.CheckFileExists("Head")) return;


        gearData = DataSerializer.instance.LoadDataFromDirectory<GearSaveData>("Head");
            
        newGear = ScriptableObject.CreateInstance<Gear>();
        newGear.CopyGear(gearData);
        newGear.ID = GetNewIndexID();
        newGear.gearSprite =GetSprite(newGear.slot);
        gd = UI_Gear.AddItemUIInventory(newGear);
        gd.OnClick();
        UI_Gear.Equip();
        
        GameManager.instance.RecheckEquipment();
    }

    private void LoadInventory()
    {
        if (DataSerializer.instance.CheckFileExists("Inventory"))
        {
            GearSaveData[] inventory = DataSerializer.instance.LoadDataFromDirectory<GearSaveData[]>("Inventory");

            for (int i = 0; i < inventory.Length; i++)
            {
                Gear newGear = ScriptableObject.CreateInstance<Gear>();
                newGear.CopyGear(inventory[i]);
                newGear.ID = GetNewIndexID();
                newGear.gearSprite = GetSprite(inventory[i].slot);
                instance.inventoryIDs.Add(newGear);
                UI_Gear.AddItemUIInventory(newGear);
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

        List<GearSaveData> datas = new List<GearSaveData>();

        if (inventoryIDs.Count > 0)
        {
            for (int i = 0; i < inventoryIDs.Count; i++)
            {
                GearSaveData data = new GearSaveData();
                data.CopyGear(inventoryIDs[i]);
                datas.Add(data);
            }
            DataSerializer.instance.SaveDataOnMainDirectory(datas.ToArray(), "Inventory");
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