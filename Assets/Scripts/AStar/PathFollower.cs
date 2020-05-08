using AStar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour {
    PathFinderData finder;
    float deltaX;
    public bool hasCoroutine { get; set; }
    IEnumerator coroutine;
    // UNDONE 完成寻路
    // A星算法只判断是否可达
    // 移动不一定需要按照算法的路径
    // 
    void Start() {
        finder = PathFinderData.Instance;
        deltaX = 3;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.F1)) {
            StartCoroutine(Tester());
        }
    }

    public void FollowPath(List<Node> nodes, EnemyManager manager) {
        print("fp manager name:" + manager.name);
        foreach (var node in nodes) {
            print("node in follower :" + node.ID);
        }
        coroutine = Follower(nodes, manager);
        hasCoroutine = true;
        StartCoroutine(coroutine);
    }

    public void StopFollow() {
        if (coroutine != null) {
            StopCoroutine(coroutine);
            hasCoroutine = false;
        } else {
            print("has no coroutine started...");
        }

    }

    IEnumerator OldFollower(List<Node> nodes, EnemyManager manager) {
        print("start follower");
        Vector3 pos = manager.enemy.transform.position;
        for (int i = 0; i < nodes.Count; i++) {
            print("move to node " + nodes[i].ID);
            Vector3 NextNodePos = nodes[i].Position;
            while (!Check(manager.enemy.transform.position, NextNodePos)) {
                pos = manager.enemy.transform.position;
                if (Vector3.Distance(pos, NextNodePos) < 2.5f) {
                    break;
                }
                if (Check(manager.enemy.transform.position, Managers.Player.player.transform.position)) {
                    goto Finded;
                }
                if (manager.isFacingRight) {
                    if (pos.x < NextNodePos.x) {
                        manager.Move();
                    } else {
                        manager.Turn();
                        manager.Move();
                    }
                } else {
                    if (pos.x < NextNodePos.x) {
                        manager.Turn();
                        manager.Move();
                    } else {
                        manager.Move();
                    }
                }
                yield return new WaitForSeconds(0.2f);
            }
        }
        Finded:
        manager.animator.SetFloat(EAParameters.SPEED, -1f);
        StartAction();
    }


    IEnumerator Tester() {

        yield return null;
    }
    IEnumerator Follower(List<Node> nodes, EnemyManager manager) {
        print("start follower " + manager.name);
        print("go name " + gameObject.name);
        Vector3 goal = manager.enemy.transform.position;
        for (int i = 0; i < nodes.Count; i++) {
            Node a = nodes[i];
            if (nodes.Count == 1) {
                goal = a.Position;
            } else {
                if (i + 1 == nodes.Count) {
                    continue;
                }
                Node b = nodes[i + 1];
                if (IsFlat(a, b)) {
                    goal.x += b.Position.x - a.Position.x;
                    continue;
                } else {
                    while (!Check(manager.enemy.transform.position, b.Position)) {
                        if (!manager.Equals(Managers.managers.GetManager(gameObject.name))) {
                            Debug.LogError("manager not same should be " + gameObject.name + " but have:" + manager.name);
                        }
                        print("follow going manager name:" + manager.name);
                        if(Check(manager.enemy.transform.position, Managers.Player.player.transform.position)) {
                            print("hit!!!");
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
                    print("follow complet");
                    goal = b.Position;
                    continue;
                }
            }
            while (!Check(manager.enemy.transform.position, goal)) {
                if (Check(manager.enemy.transform.position, Managers.Player.player.transform.position)) {
                    print("hit!!!");
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
        }
        End:
        hasCoroutine = false;
        manager.animator.SetFloat(EAParameters.SPEED, -1);
        yield return null;
    }

    bool IsFlat(Node a, Node b) {
        return Mathf.Abs(a.Position.y - b.Position.y) < 1.5f;
    }

    void StartAction() {
        print("start action");

    }

    bool Check(Vector3 currPos, Vector3 goalPos, float deviationX = 2.5f, float deviationY = 2.5f) {
        print("Checking pos:" + currPos + " pos:" + goalPos+" tag:"+tag);
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
