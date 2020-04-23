using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]

public class EnemyDetector : MonoBehaviour, IEnemyDetector
{
    public List<string> EnemyList { get;}

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.name.Contains("E_")) {
            print("enemy in " + collider.gameObject.name);
            EnemyList.Add(collider.gameObject.name);
        }
        
    }
    void OnTriggerExit2D(Collider2D collider) {
        if (collider.gameObject.name.Contains("E_")) {
            print("enemy out " + collider.gameObject.name);
            EnemyList.Remove(collider.gameObject.name);
        }
        
    }
}
