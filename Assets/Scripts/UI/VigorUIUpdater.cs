using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VigorUIUpdater : MonoBehaviour
{
    TMP_Text text;
    void Start() {
        text = GetComponent<TMP_Text>();
    }

    void Update() {
        if (int.Parse(text.text) != Managers.Player.Vigor) {
            text.text = Managers.Player.Vigor.ToString();
        }
    }
}
