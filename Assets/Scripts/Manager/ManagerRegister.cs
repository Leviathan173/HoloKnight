using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerRegister : MonoBehaviour
{
    public static EnemyManager manager;

    public void Register() {
        print("register name "+gameObject.name);
        if (gameObject.name.Contains("E_")) {
            manager = GetComponent<EnemyManager>();
            Managers.managers.AddManager(gameObject.name, manager);
        }

    }
}
