using AStar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    public enum ActionMode {
        Aggressive,
        Defence
    }
    // UNDONE 完成行动模式
    // 分为进攻和防守两个模式，防守移动较少会等待玩家进入攻击距离然后再发起进攻
    // 进攻会积极的接近距离发动攻击
    // 还有一个观望或者叫做牵扯模式
    // 在精力槽变少时稍微拉开距离等待精力回复，回复之后继续按照上述模式行动

    public bool hasCorountine { get; set; }
    IEnumerator coroutine;

    void Start() {
        hasCorountine = false;
    }

    public void StartAction(EnemyManager manager, ActionMode mode) {
        if (mode == ActionMode.Aggressive)
            coroutine = ActionAggressive(manager);
        else
            coroutine = ActionDefence(manager);
        StartCoroutine(coroutine);
        hasCorountine = true;
    }
    public void StopAction() {
        if (coroutine != null) {
            StopCoroutine(coroutine);
        } else {
            print("has no Action started");
        }
        hasCorountine = false;
    }


    IEnumerator ActionAggressive(EnemyManager manager) {
        float Hp = manager.currentHealth;
        float Sp = manager.currentStamina;
        yield return null;
    }
    IEnumerator ActionDefence(EnemyManager manager) {
        float Hp = manager.currentHealth;
        float Sp = manager.currentStamina;
        yield return null;
    }
}
