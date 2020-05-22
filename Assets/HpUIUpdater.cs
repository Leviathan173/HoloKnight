using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HpUIUpdater : MonoBehaviour
{
    TMP_Text text;
    void Start() {
        text = GetComponent<TMP_Text>();
    }

    void Update() {
        print("hp text.text.Split('/')[0]:" + text.text.Split('/')[0]);
        if (float.Parse(text.text.Split('/')[0]) != Managers.Player.currentHealth) {
            text.text = ((int)Managers.Player.currentHealth).ToString();
            text.text += "/" + Managers.Player.maxHealth;
        }
    }
}
