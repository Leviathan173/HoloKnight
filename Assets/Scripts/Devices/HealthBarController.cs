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
    EnemyManager manager = null;
    bool hasCoroutine = false;
    int time = 0;
    void Start() {
        rootName = transform.parent.gameObject.transform.parent.gameObject.name;
        healthBar = GetComponentInChildren<Image>();
        damage = GetComponentInChildren<TMP_Text>();
        manager = (EnemyManager)Managers.managers.GetManager(rootName);
        if(manager == null) {
            StartCoroutine(GetManager());
        }
        healthBar.gameObject.SetActive(false);
        damage.gameObject.SetActive(false);
    }

    IEnumerator GetManager() {
        while(manager == null) {
            yield return new WaitForSeconds(0.1f);
            manager = (EnemyManager)Managers.managers.GetManager(rootName);
        }
    }

    public void SetHealth(float value) {
        print("health maxHP:"+manager.maxHealth);
        float maxHealth = manager.maxHealth;
        float healthPercentage = value / maxHealth;
        healthBar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,healthPercentage);
        damage.text = value.ToString();
        healthBar.gameObject.SetActive(true);
        damage.gameObject.SetActive(true);
        if (!hasCoroutine) {
            time += 3;
            hasCoroutine = true;
            StartCoroutine(HideHealthBar(time));
        } else {
            time += 3;
        }
        
    }

    IEnumerator HideHealthBar(int leftTime) {
        yield return new WaitForSeconds(leftTime);
        while (time > leftTime) {
            yield return new WaitForSeconds(3);
            leftTime += 3;
        }
        healthBar.gameObject.SetActive(false);
        damage.gameObject.SetActive(false);
        hasCoroutine = false;
    }
}
