using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public enum ArmorType {
    Helmet,
    Armor,
    Boot
}
public class InventoryManager : MonoBehaviour, IGameManager {
    public ManagerStatus status { get; private set; }

    private Dictionary<int, Weapon> weapon;
    public int WeaponNextId = 0;
    private Dictionary<int, Armor> armor;
    public int ArmorNextId = 0;
    public int equippedWeapon { get; private set; }

    public void Startup() {
        Debug.Log("Inventory manager starting...");

        weapon = new Dictionary<int, Weapon>();
        armor = new Dictionary<int, Armor>();

        // any long-running startup tasks go here, and set status to 'Initializing' until those tasks are complete
        status = ManagerStatus.Started;
    }

    private void DisplayItems() {
        string itemDisplay = "Weapons: ";
        foreach (KeyValuePair<int, Weapon> item in weapon) {
            itemDisplay += item.Key + "(" + item.Value + ") ";
        }
        print(itemDisplay);
        itemDisplay = "Armors:";
        foreach (KeyValuePair<int, Weapon> item in weapon) {
            itemDisplay += item.Key + "(" + item.Value + ") ";
        }
        print(itemDisplay);
    }

    public void AddWeapon(Weapon weapon) {
        //this.weapon.Add(weapon.Id, weapon);

        DisplayItems();
    }
    public void AddArmor(Armor armor) {
        //this.armor.Add(armor.Id, armor);

        DisplayItems();
    }

    //public bool ConsumeItem(string name) {
    //    if (weapon.ContainsKey(name)) {
    //        weapon[name]--;
    //        if (weapon[name] == 0) {
    //            weapon.Remove(name);
    //        }
    //    } else {
    //        Debug.Log("cannot consume " + name);
    //        return false;
    //    }

    //    DisplayItems();
    //    return true;
    //}

    //public List<string> GetItemList() {
    //    List<string> list = new List<string>(weapon.Keys);
    //    return list;
    //}

    //public int GetItemCount(string name) {
    //    if (weapon.ContainsKey(name)) {
    //        return weapon[name];
    //    }
    //    return 0;
    //}

    public bool EquipWeapon(int id) {
        if (weapon.ContainsKey(id) && equippedWeapon != id) {
            // UNDONE 装备武器
            equippedWeapon = id;
            Debug.Log("Equipped " + id);
            return true;
        }

        equippedWeapon = -1;
        Debug.Log("Unequipped");
        return false;
    }
    public bool EquipArmor(int id, ArmorType type) {
        if (armor.ContainsKey(id) && armor[id].Type == type) {
            // UNDONE 装备特定类型装备
            Debug.Log("Equipped " + id);
            return true;
        }

        equippedWeapon = -1;
        Debug.Log("Unequipped");
        return false;
    }
}