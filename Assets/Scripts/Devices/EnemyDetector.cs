using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]

public class EnemyDetector : MonoBehaviour, IEnemyDetector {
    public List<string> EnemyList { get; set; }
    public bool hasPlayer { get; set; }

    void Awake() {
        EnemyList = new List<string>();
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.name.Contains("E_")) {
            print("enemy in " + collider.gameObject.name);
            EnemyList.Add(collider.gameObject.name);
        }else if (collider.gameObject.name.Contains("Play")) {
            print("player in" + collider.gameObject.name);
            hasPlayer = true;
        }
        
        
    }
    void OnTriggerExit2D(Collider2D collider) {
        if (collider.gameObject.name.Contains("E_")) {
            print("enemy out " + collider.gameObject.name);
            EnemyList.Remove(collider.gameObject.name);
        } else if (collider.gameObject.name.Contains("Play")) {
            print("player out" + collider.gameObject.name);
            hasPlayer = false;
        }

    }
}
