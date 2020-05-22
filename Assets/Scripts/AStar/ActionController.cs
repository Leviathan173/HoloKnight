using AStar;
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
    public void StartAction(EnemyManager manager, ActionMode mode, bool hasShield) {
        if (mode == ActionMode.Aggressive)
            coroutine = ActionAggressive(manager);
        else
            coroutine = ActionDefence(manager, hasShield);
        StartCoroutine(coroutine);
        hasCorountine = true;
    }
    /// <summary>
    /// 停止行动
    /// </summary>
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
            yield return new WaitForSeconds(0.2f);
            print("ac Get close");
            while (!Check(manager,"ac get close")) {
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
            print("ac fall down to attack");
            Attack:
            print("ac Attack");
            Sp = manager.currentStamina;
            print("ac attack ready sp :" + Sp);
            if (Sp >= manager.attackCost) {
                float ran = Random.Range(0, 2);
                print("ac random :" + ran);
                if(ran < 1) {
                    print("ac attack a");
                    yield return new WaitForSeconds(0.3f);
                    manager.AttackAEnter();
                } else {
                    print("ac attack b");
                    yield return new WaitForSeconds(0.3f);
                    manager.AttackBEnter();
                }
                yield return new WaitForSeconds(2);
                if(manager.currentStamina > manager.attackCost) {
                    goto GetClose;
                } else {
                    goto Hold;
                }
                
            } else {
                print("ac has no sp");
                goto Hold;
            }
            Hold:
            yield return new WaitForSeconds(0.2f);
            print("ac hold");
            while(Check(manager, "ac hold")) {
                if (manager.isFacingRight) {
                    if (manager.enemy.transform.position.x < Managers.Player.player.transform.position.x) {
                        manager.Move(-1);
                    } else {
                        manager.Turn();
                        manager.Move(-1);
                    }
                } else {
                    if (manager.enemy.transform.position.x < Managers.Player.player.transform.position.x) {
                        manager.Turn();
                        manager.Move(-1);
                    } else {
                        manager.Move(-1);
                    }
                }
                print("ac hold wait");
                yield return new WaitForSeconds(0.2f);
                if (manager.currentStamina >= manager.attackCost) {
                    print("ac hold has sp");
                    goto GetClose;
                }
            }
        }
    }
    /// <summary>
    /// 偏向防守的进攻模式
    /// </summary>
    /// <param name="manager">敌人管理器</param>
    /// <returns></returns>
    IEnumerator ActionDefence(EnemyManager manager, bool hasShield) {
        float Hp = manager.currentHealth;
        float Sp = manager.currentStamina;
        while (true) {
            GetClose:
            Hp = manager.currentHealth;
            if (Hp > manager.maxHealth / 2) {
                print("ac Get close");
                while (!Check(manager,"ac get close")) {
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
            }
            Defence:
            float ran = Random.Range(0, 100) / 100.0f;
            print("ac ran :" + ran + "health :" + manager.currentHealth / manager.maxHealth);
            if(ran > manager.currentHealth/manager.maxHealth) {
                print("ac defence");
                if (hasShield) {
                    manager.UseShield();
                    yield return new WaitForSeconds(2);
                    goto GetClose;
                } else {
                    goto Hold;
                }
            }
            Attack:
            print("ac Attack");
            manager.UnuseShield();
            Sp = manager.currentStamina;
            if (Sp > manager.attackCost) {
                ran = Random.Range(0, 2);
                print("ac random :" + ran);
                if (ran < 1) {
                    manager.AttackAEnter();
                } else {
                    manager.AttackBEnter();
                }
                yield return new WaitForSeconds(2);
                goto GetClose;
            } else {
                goto Hold;
            }
            Hold:
            print("ac hold");
            while (Check(manager,"ac hold")) {
                if (manager.isFacingRight) {
                    if (manager.enemy.transform.position.x < Managers.Player.player.transform.position.x) {
                        manager.Move(-1);
                    } else {
                        manager.Turn();
                        manager.Move(-1);
                    }
                } else {
                    if (manager.enemy.transform.position.x < Managers.Player.player.transform.position.x) {
                        manager.Turn();
                        manager.Move(-1);
                    } else {
                        manager.Move(-1);
                    }
                }
                yield return new WaitForSeconds(0.2f);
                ran = Random.Range(0, 5);
                if(ran == 0) {
                    goto Defence;
                }
                if (manager.currentStamina > manager.attackCost) {
                    goto GetClose;
                }
            }
            yield return new WaitForSeconds(1.5f);
        }
    }

    bool Check(EnemyManager manager, string msg = "no msg") {
        print("checking " + msg);
        foreach(var detector in manager.Attacks) {
            if (detector.hasPlayer) {
                print("checked in " + msg);
                return true;
            }
        }
        print("failed to check " + msg);
        return false;
    }
}
