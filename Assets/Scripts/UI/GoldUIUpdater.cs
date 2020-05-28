using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoldUIUpdater : MonoBehaviour
{
    TMP_Text text;
    void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    void Update()
    {
        if (int.Parse(text.text) != Managers.Player.gold) {
            text.text = Managers.Player.gold.ToString();
        }
    }
}
