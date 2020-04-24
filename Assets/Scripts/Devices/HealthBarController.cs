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
    IGameManager manager = null;
    bool hasCoroutine = false;
    int time = 0;
    void Start() {
        rootName = transform.parent.gameObject.transform.parent.gameObject.name;
        healthBar = GetComponentInChildren<Image>();
        damage = GetComponentInChildren<TMP_Text>();
        print("managers:" + Managers.managers);
        manager = Managers.managers.GetManager(rootName);
        healthBar.gameObject.SetActive(false);
        damage.gameObject.SetActive(false);
    }

    public void SetHealth(float value) {
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
            yield return new WaitForSeconds(time - leftTime);
        }
        healthBar.gameObject.SetActive(false);
        damage.gameObject.SetActive(false);
        hasCoroutine = false;
    }
}
