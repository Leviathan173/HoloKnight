﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerRegister : MonoBehaviour
{
    public EnemyManager manager = null;

    public void Register() {
        print("register name "+gameObject.name);
        if (gameObject.name.Contains("E_")) {
            manager = GetComponent<EnemyManager>();
            print("enemy manager name :" + manager.name);
            Managers.managers.AddManager(gameObject.name, manager);
        }

    }
}
