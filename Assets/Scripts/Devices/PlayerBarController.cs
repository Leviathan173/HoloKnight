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
    public void UpdateHealth() {
        float healthPercentage = Managers.Player.currentHealth / Managers.Player.maxHealth;
        healthBar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,healthPercentage * 200);
    }
    public void UpdateSp() {
        float spPercentage = Managers.Player.currentStamina / Managers.Player.maxStamina;
        spBar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, spPercentage * 200);
    }
}
