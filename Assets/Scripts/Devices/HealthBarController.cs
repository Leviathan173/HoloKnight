using System;
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
    [SerializeField] EnemyManager manager = null;
    bool hasCoroutine = false;
    float totalDamage = 0;
    float currrentHealth = 0;
    float maxHealth = 0;
    float time = 0;
    void Start() {
        rootName = transform.parent.gameObject.transform.parent.gameObject.name;
        healthBar = GetComponentInChildren<Image>();
        damage = GetComponentInChildren<TMP_Text>();
        manager = (EnemyManager)Managers.managers.GetManager(rootName);
        StartCoroutine(GetManager());
        healthBar.gameObject.SetActive(false);
        damage.gameObject.SetActive(false);
    }

    IEnumerator GetManager() {
        while(manager == null) {
            yield return new WaitForSeconds(0.1f);
            manager = (EnemyManager)Managers.managers.GetManager(rootName);
        }
        maxHealth = manager.maxHealth;
        currrentHealth = maxHealth;
    }

    public void SetHealth(float value) {
        currrentHealth -= value;
        float healthPercentage = currrentHealth / maxHealth;
        healthBar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,healthPercentage);
        totalDamage += value;
        damage.text = totalDamage.ToString();
        healthBar.gameObject.SetActive(true);
        damage.gameObject.SetActive(true);
        if (!hasCoroutine) {
            time = Time.time + 3;
            hasCoroutine = true;
            StartCoroutine(HideHealthBar());
        } else {
            time = Time.time + 3;
        }
        
    }

    IEnumerator HideHealthBar() {
        while (time > Time.time) {
            yield return new WaitForSeconds(1);
        }
        healthBar.gameObject.SetActive(false);
        damage.gameObject.SetActive(false);
        totalDamage = 0;
        hasCoroutine = false;
    }
}
