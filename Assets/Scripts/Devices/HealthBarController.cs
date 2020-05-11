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
    /// <summary>
    /// 获取管理器的协程，因为这个脚本会比管理器初始化更先启动
    /// </summary>
    /// <returns></returns>
    IEnumerator GetManager() {
        while(manager == null) {
            yield return new WaitForSeconds(0.1f);
            manager = (EnemyManager)Managers.managers.GetManager(rootName);
        }
        maxHealth = manager.maxHealth;
        currrentHealth = maxHealth;
    }
    /// <summary>
    /// 设置血条的长度
    /// </summary>
    /// <param name="value">血量减少的值</param>
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
    /// <summary>
    /// 在不受到伤害的三秒后隐藏血条
    /// </summary>
    /// <returns></returns>
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
