﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPicker : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.name.Contains("Player")) {
            Managers.Audio.PlayClipOneShot(Managers.Audio.coin);
            Managers.Player.hasKey = true;
            Destroy(gameObject);
        }
    }
}
