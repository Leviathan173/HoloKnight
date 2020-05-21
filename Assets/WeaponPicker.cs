using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class WeaponPicker : MonoBehaviour
{
    [SerializeField] string name;
    [SerializeField] float attack;
    [SerializeField] int level;
    private Weapon weapon;
    void Start()
    {
        weapon = new Weapon(name, attack, level);
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.name.Contains("Player")) {
            if (Managers.Player.weapon.Name.Equals(weapon.Name)) {
                Managers.Player.weapon.Level += 1;
                print("weapon " + weapon.Name + " levelup:" + Managers.Player.weapon.Level);
                Destroy(gameObject);
            } else {
                Managers.Player.weapon = weapon;
                print("switch to new weapon " + weapon.Name);
                Destroy(gameObject);
            }
        }
    }
}
