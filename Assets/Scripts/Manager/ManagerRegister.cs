using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerRegister : MonoBehaviour
{
    

    public void Register() {
        EnemyManager manager;
        //print("register name "+gameObject.name);
        if (gameObject.name.Contains("E_")) {
            manager = GetComponent<EnemyManager>();
            //print("enemy manager name :" + manager.name);
            Managers.managers.AddManager(gameObject.name, manager);
        }

    }
}
