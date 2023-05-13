using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private static Inventory instance;

    [SerializeField] private Gear[] gearsDatas;

    private List<int> inventoryIDs = new List<int>();
    private Dictionary<int, Gear> dicoGear = new Dictionary<int, Gear>();

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        foreach (var gear in gearsDatas)
        {
            dicoGear.Add(gear.ID, gear);
        }

        LoadEquipment(LoadInventory());
    }
    
    public static Gear[] GetGearsData()
    {
        return instance.gearsDatas;
    }

    public static GearDescription AddItemOnInventory(int ID)
    {
        instance.dicoGear.TryGetValue(ID, out Gear gear);
        instance.inventoryIDs.Add(ID);
        return UI_Gear.AddItemUIInventory(gear);
    }

    public static void RemoveItemOnInventory(int ID)
    {
        instance.inventoryIDs.Remove(ID);

        UI_Gear.RemoveItemUIInventory(instance.dicoGear[ID]);
    }

    private async void LoadEquipment(List<GearDescription> gears)
    {
        await Task.Delay(200);
        if (PlayerPrefs.HasKey("Weapon"))
        {
            int weaponID = PlayerPrefs.GetInt("Weapon");
            foreach (var gear in gears.Where(gear => gear.gear.ID == weaponID))
            {
                gear.OnClick();
                UI_Gear.Equip();
                break;
            }
        }

        if (PlayerPrefs.HasKey("Chest"))
        {
            int chestID = PlayerPrefs.GetInt("Chest");
            foreach (var gear in gears.Where(gear => gear.gear.ID == chestID))
            {
                gear.OnClick();
                UI_Gear.Equip();
                break;
            }
        }

        if (!PlayerPrefs.HasKey("Head")) return;

        int legsID = PlayerPrefs.GetInt("Head");

        foreach (var gear in gears.Where(gear => gear.gear.ID == legsID))
        {
            gear.OnClick();
            UI_Gear.Equip();
            break;
        }
    }

    private List<GearDescription> GetDefaultInventory()
    {
        List<GearDescription> gears = new List<GearDescription>();

        for (int i = 0; i < 4; i++)
        {
            gears.Add(AddItemOnInventory(i));
        }

        return gears;
    }

    private List<GearDescription> LoadInventory()
    {
        List<GearDescription> gears = new List<GearDescription>();

        if (PlayerPrefs.HasKey("Inventory"))
        {
            string inventory = PlayerPrefs.GetString("Inventory");
            string[] inventorySplit = inventory.Split(';');

            if (inventory.Length > 0)
                foreach (var t in inventorySplit)
                {
                    var ID = int.Parse(t);
                    gears.Add(AddItemOnInventory(ID));
                }
            else
                return GetDefaultInventory();
        }
        else
            return GetDefaultInventory();

        return gears;
    }

    public static GameObject DropInventory(Rarity rarity)
    {
        List<Gear> gears = new List<Gear>();
        foreach (var gear in instance.gearsDatas)
        {
            if (gear.rarity == rarity)
                gears.Add(gear);
        }
        
        int random = UnityEngine.Random.Range(0, gears.Count);
        AddItemOnInventory(gears[random].ID);
        return UI_Gear.DropItem(gears[random]);
    }

    private void OnDestroy()
    {
        string inventory = "";

        for (var i = 0; i < inventoryIDs.Count; i++)
        {
            inventory += inventoryIDs[i];
            if (i + 1 < inventoryIDs.Count)
                inventory += ";";
        }

        PlayerPrefs.SetString("Inventory", inventory);
    }
}