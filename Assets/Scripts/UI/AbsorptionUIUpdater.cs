using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AbsorptionUIUpdater : MonoBehaviour
{
    TMP_Text text;
    void Start() {
        text = GetComponent<TMP_Text>();
    }

    void Update() {
        int a = (int)(100 / (Managers.Player.defence / 100));
        string s = a + "%"; 
        if (!text.text.Equals(s)) {
            text.text = s;
        }
    }
}
