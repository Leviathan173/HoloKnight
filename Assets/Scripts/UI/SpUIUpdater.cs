using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpUIUpdater : MonoBehaviour
{
    TMP_Text text;
    void Start() {
        text = GetComponent<TMP_Text>();
    }

    void Update() {
        print("sp text.text.Split('/')[0]:" + text.text.Split('/')[0]);
        if (float.Parse(text.text.Split('/')[0]) != Managers.Player.currentStamina) {
            text.text = ((int)Managers.Player.currentStamina).ToString();
            text.text += "/" + ((int)Managers.Player.maxStamina).ToString();
        }
    }
}
