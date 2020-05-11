﻿using AStar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour {
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
    /// <summary>
    /// 开始行动模式
    /// </summary>
    /// <param name="manager">敌人管理器</param>
    /// <param name="mode">进攻模式</param>
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

    /// <summary>
    /// 积极的进攻模式
    /// </summary>
    /// <param name="manager">敌人管理器</param>
    /// <returns></returns>
    IEnumerator ActionAggressive(EnemyManager manager) {
        float Sp;
        while (true) {
            GetClose:
            print("Get close");
            while (!manager.follower.Check(manager.enemy.transform.position,
                Managers.Player.player.transform.position,
                "get close", 1, 1)) {
                if (manager.isFacingRight) {
                    if (manager.enemy.transform.position.x < Managers.Player.player.transform.position.x) {
                        manager.Move();
                    } else {
                        manager.Turn();
                        manager.Move();
                    }
                } else {
                    if (manager.enemy.transform.position.x < Managers.Player.player.transform.position.x) {
                        manager.Turn();
                        manager.Move();
                    } else {
                        manager.Move();
                    }
                }
                yield return new WaitForSeconds(0.2f);
            }
            Attack:
            print("Attack");
            Sp = manager.currentStamina;
            if (Sp > manager.attackCost) {
                float ran = Random.Range(0, 2);
                print("random :" + ran);
                if(ran < 1) {
                    manager.AttackAEnter();
                    manager.AttackAExit();
                } else {
                    manager.AttackBEnter();
                    manager.AttackBExit();
                }
                goto GetClose;
            } else {
                goto Hold;
            }
            Hold:
            print("hold");
            while(manager.follower.Check(manager.enemy.transform.position,
                Managers.Player.player.transform.position,
                "hold")) {
                if (manager.isFacingRight) {
                    if (manager.enemy.transform.position.x < Managers.Player.player.transform.position.x) {
                        manager.Move(-1);
                    } else {
                        manager.Turn();
                        manager.Move();
                    }
                } else {
                    if (manager.enemy.transform.position.x < Managers.Player.player.transform.position.x) {
                        manager.Turn();
                        manager.Move(-1);
                    } else {
                        manager.Move();
                    }
                }
                yield return new WaitForSeconds(0.2f);
                if (manager.currentStamina > manager.attackCost) {
                    goto GetClose;
                }
            }
        }
        yield return null;
    }
    /// <summary>
    /// 偏向防守的进攻模式
    /// </summary>
    /// <param name="manager">敌人管理器</param>
    /// <returns></returns>
    IEnumerator ActionDefence(EnemyManager manager) {
        float Hp = manager.currentHealth;
        float Sp = manager.currentStamina;
        yield return null;
    }
}
