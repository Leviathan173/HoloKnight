﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Gold : MonoBehaviour
{
    TMP_Text text;
    public int gold = 0;
    bool hasCoroutine = false;
    int to = 0;
    void Start()
    {
        gold = Managers.Player.gold;
        text = GetComponent<TMP_Text>();
    }

    void Update()
    {
        print("player:" + Managers.Player.gold + "  " + gold + "  "+ (Managers.Player.gold != gold));
        if(Managers.Player.gold != gold) {
            if (hasCoroutine) {
                to = Managers.Player.gold;
            } else {
                gold = int.Parse(text.text);
                hasCoroutine = true;
                to = Managers.Player.gold;
                StartCoroutine(Increaser(gold));
            }
        }
    }
    IEnumerator Increaser(int from) {
        int C = 1;
        if (from > to) C = -C;
        while (from != to) {
            print("coin"+from);
            from += C;
            text.text = from.ToString();
            yield return null;
        }
        hasCoroutine = false;
    }
}
