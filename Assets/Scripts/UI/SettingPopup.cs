﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingPopup : MonoBehaviour
{
    void Awake()
    {
        gameObject.SetActive(false);
    }
    public void Open() {
        gameObject.SetActive(true);
    }
    public void Close(){
        gameObject.SetActive(false);
    }
}
