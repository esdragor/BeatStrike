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

    public static void Init()
    {

        instance.LoadInventory();
        instance.LoadEquipment();
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

    public static Sprite GetSprite(GearSlot gear)
    {
        switch (gear)
        {
            case GearSlot.Head:
                return instance.spriteHead;
                break;
            case GearSlot.Chest:
                return instance.spriteChest;
                break;
            case GearSlot.Weapon:
                return instance.spriteWeapon;
                break;
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
        if (DataSerializer.instance.CheckFileExists("Weapon"))
        {
            // newGear = Ressources.Load
            
            
            
            newGear = DataSerializer.instance.LoadDataFromDirectory<Gear>("Weapon");
            newGear.ID = GetNewIndexID();
            newGear.gearSprite = GetSprite(newGear.slot);
            //instance.inventoryIDs.Add(DataSerializer.instance.LoadDataFromDirectory<Gear>("Weapon"));
            gd = UI_Gear.AddItemUIInventory(newGear);
            gd.OnClick();
            UI_Gear.Equip();
        }

        if (DataSerializer.instance.CheckFileExists("Chest"))
        {
            newGear = DataSerializer.instance.LoadDataFromDirectory<Gear>("Chest");
            newGear.ID = GetNewIndexID();
            newGear.gearSprite =GetSprite(newGear.slot);
            //instance.inventoryIDs.Add(DataSerializer.instance.LoadDataFromDirectory<Gear>("Chest"));
            gd = UI_Gear.AddItemUIInventory(newGear);
            gd.OnClick();
            UI_Gear.Equip();
        }

        if (!DataSerializer.instance.CheckFileExists("Head")) return;


        newGear = DataSerializer.instance.LoadDataFromDirectory<Gear>("Head");
        newGear.ID = GetNewIndexID();
        newGear.gearSprite =GetSprite(newGear.slot);
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
                inventory[i].gearSprite = GetSprite(inventory[i].slot);
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