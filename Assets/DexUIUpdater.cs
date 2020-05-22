﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DexUIUpdater : MonoBehaviour
{
    TMP_Text text;
    void Start() {
        text = GetComponent<TMP_Text>();
    }

    void Update() {
        if (int.Parse(text.text) != Managers.Player.dex) {
            text.text = Managers.Player.dex.ToString();
        }
    }
}
