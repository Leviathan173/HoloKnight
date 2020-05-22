using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ArmorPicker : MonoBehaviour
{
    [SerializeField] string name;
    [SerializeField] EquipType type;
    [SerializeField] float defence;
    [SerializeField] int level;
    private Armor armor;
    void Start()
    {
        armor = new Armor(name, type, defence, level);
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.name.Contains("Player")) {
            for(int i = 0; i < Managers.Player.armors.Count; i++) {
                if (Managers.Player.armors[i].Type.Equals(type)) {
                    Managers.Audio.PlayClipOneShot(Managers.Audio.equip);
                    if (Managers.Player.armors[i].Name.Equals(name)) {
                        Managers.Player.armors[i].Level += 1;
                        print(type + " levelup " + Managers.Player.armors[i].Level);
                        Destroy(gameObject);
                    } else {
                        Managers.Player.armors[i] = armor;
                        print(type + " switch to " + name);
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}
