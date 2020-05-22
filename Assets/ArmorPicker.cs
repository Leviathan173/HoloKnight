using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ArmorPicker : MonoBehaviour
{
    [SerializeField] string aName;
    [SerializeField] EquipType type;
    [SerializeField] float defence;
    [SerializeField] int level;
    private Armor armor;
    void Start()
    {
        armor = new Armor(aName, type, defence, level);
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.name.Contains("Player")) {
            for(int i = 0; i < Managers.Player.armors.Count; i++) {
                if (Managers.Player.armors[i].Type == type) {
                    print("armors[i].type = " + Managers.Player.armors[i].Type + " armor type = " + type);
                    Managers.Audio.PlayClipOneShot(Managers.Audio.equip);
                    if (Managers.Player.armors[i].Name.Equals(aName)) {
                        print("i=" + i + " armors[i]=" + Managers.Player.armors[i].Name + "name =" + aName);
                        Managers.Player.armors[i].Level += 1;
                        //switch (armor.Type) {
                        //    case EquipType.Armor:
                        //        Managers.Player.armor.Level += 1;
                        //        break;
                        //    case EquipType.Boot:
                        //        Managers.Player.boot.Level += 1;
                        //        break;
                        //    case EquipType.Helmet:
                        //        Managers.Player.helmet.Level += 1;
                        //        break;
                        //}
                        print(type + " levelup " + Managers.Player.armors[i].Level);
                        Destroy(gameObject);
                    } else {
                        //Managers.Player.armors.Remove(Managers.Player.armors[i]);
                        //Managers.Player.armors.Add(armor);
                        
                        print("i=" + i + " armors[i]=" + Managers.Player.armors[i].Name + "name =" + aName);
                        Managers.Player.armors[i] = armor;
                        switch (armor.Type) {
                            case EquipType.Armor:
                                Managers.Player.armor = armor;
                                break;
                            case EquipType.Boot:
                                Managers.Player.boot = armor;
                                break;
                            case EquipType.Helmet:
                                Managers.Player.helmet = armor;
                                break;
                        }
                        print("i=" + i + " armors[i]=" + Managers.Player.armors[i].Name + "name =" + aName);
                        print("i=" + i + " armor=" + Managers.Player.armor.Name + "name =" + aName);
                        print("i=  armors.count=" + Managers.Player.armors.Count);
                        print(type + " switch to " + aName);
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}
