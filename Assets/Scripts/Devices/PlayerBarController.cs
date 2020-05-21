using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBarController : MonoBehaviour
{
    [SerializeField]Image healthBar;
    [SerializeField] Image spBar;
    string rootName;
    float time = 0;
    public void UpdateHealth() {
        float healthPercentage = Managers.Player.currentHealth / Managers.Player.maxHealth;
        print("health %:" + healthPercentage);
        healthBar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,healthPercentage * 200);
    }
    public void UpdateSp() {
        float spPercentage = Managers.Player.currentStamina / Managers.Player.maxStamina;
        print("spp %:" + spPercentage);
        spBar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, spPercentage * 200);
    }
}
