using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.name.Contains("Player")) {
            Managers.Audio.bgm.IntoSafeArea();
            Managers.Player.lastSpawnPos = transform.position;
        }
    }
    void OnTriggerExit2D(Collider2D collider) {
        if (collider.gameObject.name.Contains("Player")) {
            Managers.Audio.bgm.IntoDefault();
            Managers.Player.lastSpawnPos = transform.position;
        }
    }
}
