using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipUIUpdater : MonoBehaviour {
    //[SerializeField] Image image;
    [SerializeField] TMP_Text eName;
    [SerializeField] TMP_Text value;
    [SerializeField] EquipType type;
    Weapon weapon = null;
    public Armor helmet = null;
    public Armor boot = null;
    public Armor armor = null;
    void Update() {
        switch (type) {
            case EquipType.Weapon:
                weapon = Managers.Player.weapon;
                //Sprite sprite = Resources.Load("Assets/Resources/Icons/" + weapon.Name, typeof(Sprite)) as Sprite;
                //image.sprite = sprite;
                if (weapon.Level > 1) {
                    eName.text = weapon.Name + "+" + weapon.Level;
                } else {
                    eName.text = weapon.Name;
                }

                value.text = "AP:" + weapon.Attack.ToString();
                break;
            case EquipType.Armor:
                armor = Managers.Player.armor;
                //print("load path:Icons/" + armor.Name);
                //image.sprite = Resources.Load("Icons/" + armor.Name) as Sprite;
                if (armor.Level > 1) {
                    eName.text = armor.Name + "+" + armor.Level;
                } else {
                    eName.text = armor.Name;
                }
                value.text = "Def:" + armor.Defence.ToString();
                break;
            case EquipType.Helmet:
                helmet = Managers.Player.helmet;
                //print("load path:Icons/" + helmet.Name);
                //image.sprite = Resources.Load("Icons/" + helmet.Name) as Sprite;
                if (helmet.Level > 1) {
                    eName.text = helmet.Name + "+" + helmet.Level;
                } else {
                    eName.text = helmet.Name;
                }
                value.text = "Def:" + helmet.Defence.ToString();
                break;
            case EquipType.Boot:
                boot = Managers.Player.boot;
                //print("load path:Icons/" + boot.Name);
                //image.sprite = Resources.Load("Icons/" + boot.Name) as Sprite;
                if (boot.Level > 1) {
                    eName.text = boot.Name + "+" + boot.Level;
                } else {
                    eName.text = boot.Name;
                }
                value.text = "Def:" + boot.Defence.ToString();
                break;
        }
    }
}
