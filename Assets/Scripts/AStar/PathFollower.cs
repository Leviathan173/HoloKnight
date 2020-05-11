using AStar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour {
    public bool hasCoroutine { get; set; }
    IEnumerator coroutine;
    void Start() {
        hasCoroutine = false;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.F1)) {
            StartCoroutine(Tester());
        }
    }
    /// <summary>
    /// 跟寻路径
    /// </summary>
    /// <param name="nodes">路径中的顺序结点</param>
    /// <param name="manager">敌人管理器</param>
    public void FollowPath(List<Node> nodes, EnemyManager manager) {
        coroutine = Follower(nodes, manager);
        hasCoroutine = true;
        StartCoroutine(coroutine);
    }
    /// <summary>
    /// 停止跟寻路线
    /// </summary>
    public void StopFollow() {
        if (coroutine != null) {
            StopCoroutine(coroutine);
            hasCoroutine = false;
        } else {
            print("has no Follower started...");
        }

    }

    //IEnumerator OldFollower(List<Node> nodes, EnemyManager manager) {
    //    print("start follower");
    //    Vector3 pos = manager.enemy.transform.position;
    //    for (int i = 0; i < nodes.Count; i++) {
    //        print("move to node " + nodes[i].ID);
    //        Vector3 NextNodePos = nodes[i].Position;
    //        while (!Check(manager.enemy.transform.position, NextNodePos)) {
    //            pos = manager.enemy.transform.position;
    //            if (Vector3.Distance(pos, NextNodePos) < 2.5f) {
    //                break;
    //            }
    //            if (Check(manager.enemy.transform.position, Managers.Player.player.transform.position)) {
    //                goto Finded;
    //            }
    //            if (manager.isFacingRight) {
    //                if (pos.x < NextNodePos.x) {
    //                    manager.Move();
    //                } else {
    //                    manager.Turn();
    //                    manager.Move();
    //                }
    //            } else {
    //                if (pos.x < NextNodePos.x) {
    //                    manager.Turn();
    //                    manager.Move();
    //                } else {
    //                    manager.Move();
    //                }
    //            }
    //            yield return new WaitForSeconds(0.2f);
    //        }
    //    }
    //    Finded:
    //    manager.animator.SetFloat(EAParameters.SPEED, -1f);
    //    StartAction();
    //}


    IEnumerator Tester() {

        yield return null;
    }
    /// <summary>
    /// 跟寻协程，控制行动逻辑
    /// </summary>
    /// <param name="nodes">路径中的顺序结点</param>
    /// <param name="manager">敌人管理器</param>
    /// <returns></returns>
    IEnumerator Follower(List<Node> nodes, EnemyManager manager) {
        Vector3 goal = nodes[0].Position;
        for (int i = 0; i < nodes.Count; i++) {
            //print("pf loop " + i + "name:" + manager.name);
            //print("pf node id " + nodes[i].ID + "name:" + manager.name);
            Node a = nodes[i];
            if (nodes.Count == 1) {
                //print("pf only one node" + " name:" + manager.name);
                goal = a.Position;
            } else {
                if (i + 1 == nodes.Count) {
                    //print("pf reach last node" + " name:" + manager.name);
                    continue;
                }
                Node b = nodes[i + 1];
                if (IsFlat(a, b)) {
                    //print("pf is flat" + " name:" + manager.name);
                    goal.x += b.Position.x - a.Position.x;
                    //print("pf goal " + goal + " name:" + manager.name);
                    continue;
                } else {
                    //print("pf isn't flat" + " name:" + manager.name);
                    goal = b.Position;
                    while (!Check(manager.enemy.transform.position, b.Position, manager.name)) {
                        if (Check(manager.enemy.transform.position, Managers.Player.player.transform.position, manager.name)) {
                            //print("pf hit!!!" + " name:" + manager.name);
                            goto End;
                        }
                        if (manager.isFacingRight) {
                            if (manager.enemy.transform.position.x < goal.x) {
                                manager.Move();
                            } else {
                                manager.Turn();
                                manager.Move();
                            }
                        } else {
                            if (manager.enemy.transform.position.x < goal.x) {
                                manager.Turn();
                                manager.Move();
                            } else {
                                manager.Move();
                            }
                        }
                        yield return new WaitForSeconds(0.2f);
                    }
                    //print("pf follow complet" + " name:" + manager.name);
                    goal = b.Position;
                    continue;
                }
            }
        }
        Check:
        //print("pf check last " + "name:" + manager.name);
        while (!Check(manager.enemy.transform.position, goal, manager.name)) {
            //print("pf into last check " + "name:" + manager.name);
            if (Check(manager.enemy.transform.position, Managers.Player.player.transform.position, manager.name)) {
                //print("pf hit!!!" + " name:" + manager.name);
                goto End;
            }
            if (manager.isFacingRight) {
                if (manager.enemy.transform.position.x < goal.x) {
                    manager.Move();
                } else {
                    manager.Turn();
                    manager.Move();
                }
            } else {
                if (manager.enemy.transform.position.x < goal.x) {
                    manager.Turn();
                    manager.Move();
                } else {
                    manager.Move();
                }
            }
            yield return new WaitForSeconds(0.2f);
        }
        if (!Check(Managers.Player.player.transform.position, goal, "player")) {
            if(Vector3.Distance(Managers.Player.player.transform.position, goal) < 10) {
                goal = Managers.Player.player.transform.position;
                goto Check;
            }
        }
        End:
        //print("pf end " + "name:" + manager.name);
        hasCoroutine = false;
        manager.animator.SetFloat(EAParameters.SPEED, -1);
        yield return null;
    }
    /// <summary>
    /// a、b两点是否在同一平面上
    /// </summary>
    /// <param name="a">结点A</param>
    /// <param name="b">结点B</param>
    /// <returns></returns>
    bool IsFlat(Node a, Node b) {
        return Mathf.Abs(a.Position.y - b.Position.y) < 1.5f;
    }
    /// <summary>
    /// 检查两点的距离是否在误差之间
    /// </summary>
    /// <param name="currPos">现在位置</param>
    /// <param name="goalPos">终点位置</param>
    /// <param name="tag">Debug用字符串</param>
    /// <param name="deviationX">X轴的可接受误差</param>
    /// <param name="deviationY">Y轴的可接受误差</param>
    /// <returns></returns>
    bool Check(Vector3 currPos, Vector3 goalPos, string tag, float deviationX = 2.5f, float deviationY = 2.5f) {
        //print("pf Checking pos:" + currPos + " pos:" + goalPos + " tag:" + tag);
        if (Mathf.Abs(currPos.x - goalPos.x) < deviationX) {
            if (Mathf.Abs(currPos.y - goalPos.y) < deviationY) {
                print("on goal currpos:" + currPos + " goalPos:" + goalPos);
                return true;
            } else {
                return false;
            }
        } else {
            return false;
        }
    }
}
