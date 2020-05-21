using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionPicker : MonoBehaviour
{
    [SerializeField]public float heal = 35;
    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.name.Contains("Player")) {
            Managers.Player.Heal(heal);
            Destroy(gameObject);
        }
    }
}
