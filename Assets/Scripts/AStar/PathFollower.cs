using AStar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour
{
    [SerializeField] EnemyManager manager;
    PathFinder finder;
    float deltaX;
    public bool hasCoroutine { get; set; }
    IEnumerator coroutine;
    // UNDONE 完成寻路
    // A星算法只判断是否可达
    // 移动不一定需要按照算法的路径
    // 
    void Start()
    {
        manager = (EnemyManager)Managers.managers.GetManager(gameObject.name);
        finder = PathFinder.Instance;
        deltaX = 3;
    }

    public void FollowPath(List<Node> nodes) {
        foreach(var node in nodes) {
            print("node in follower :" + node.ID);
        }
        coroutine = Follower(nodes);
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

    IEnumerator Follower(List<Node> nodes) {
        print("start follower");
        Vector3 pos = manager.enemy.transform.position;
        for(int i = 0; i < nodes.Count; i++) {
            print("move to node " + nodes[i].ID);
            Vector3 NextNodePos = nodes[i].Position;
            while (!Check(manager.enemy.transform.position, NextNodePos)) {
                pos = manager.enemy.transform.position;
                if (Vector3.Distance(pos, NextNodePos) < 5) {
                    goto Finded;
                }
                if(Check(manager.enemy.transform.position, Managers.Player.player.transform.position)) {
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
            StartAction();
    }

    void StartAction() {
        print("start action");
    }

    bool Check(Vector3 currPos, Vector3 goalPos, float deviationX = 5, float deviationY = 5) {
        if(currPos.x > goalPos.x - deviationX && currPos.x < goalPos.x + deviationX) {
            if(currPos.y > goalPos.y - deviationY && currPos.y < goalPos.y + deviationY) {
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
