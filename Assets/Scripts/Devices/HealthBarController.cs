using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    Image healthBar;
    TMP_Text damage;
    string rootName;
    IGameManager manager;
    void Awake() {
        rootName = GetComponentInParent<Transform>().GetComponentInParent<Transform>().gameObject.name;
        healthBar = GetComponentInChildren<Image>();
        damage = GetComponentInChildren<TMP_Text>();
        manager = Managers.managers.GetManager(rootName);
    }

    public void SetHealth(float value) {
        float maxHealth = manager.maxHealth;
        float healthPercentage = value / maxHealth;
        healthBar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,healthPercentage);
    }
}
