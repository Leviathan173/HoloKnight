using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DefenseUIUpdater : MonoBehaviour
{
    TMP_Text text;
    void Start() {
        text = GetComponent<TMP_Text>();
    }

    void Update() {
        if (int.Parse(text.text) != (int)Managers.Player.defence) {
            text.text = ((int)Managers.Player.defence).ToString();
        }
    }
}
