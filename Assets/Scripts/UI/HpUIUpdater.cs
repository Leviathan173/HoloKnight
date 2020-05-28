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
        text.text = ((int)Managers.Player.currentHealth).ToString();
        text.text += "/" + Managers.Player.maxHealth;
    }
}
