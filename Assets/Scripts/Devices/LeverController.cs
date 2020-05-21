using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverController : MonoBehaviour
{

    [SerializeField] Player player;
    [SerializeField] ElevatorController elevator;
    [SerializeField] bool needCoin = false;
    [SerializeField] public bool MoveDown = true;
    Animator animator;
    

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) {
            if (needCoin) {
                if (Managers.Player.gold >= 300) {
                    if (CheckDistance()) {
                        if (MoveDown) {
                            if (elevator.atTop) {
                                StartCoroutine(Move());
                                Managers.Player.gold -= 300;
                                needCoin = false;
                            }
                        } else {
                            if (!elevator.atTop) {
                                StartCoroutine(Move());
                                Managers.Player.gold -= 300;
                                needCoin = false;
                            }
                        }
                    }
                }
            }else if (CheckDistance()) {
                if (MoveDown) {
                    if (elevator.atTop) {
                        StartCoroutine(Move());
                    }
                } else {
                    if (!elevator.atTop) {
                        StartCoroutine(Move());
                    }
                }
            }

        }
    }
    /// <summary>
    /// 检测玩家是否面向拉杆并且在一定距离内
    /// </summary>
    /// <returns></returns>
    bool CheckDistance() {
        Vector3 direction = player.transform.position - transform.position;
        print(elevator.name+":"+":"+gameObject.name+" "+Vector3.Dot(transform.forward, direction.normalized));
        if (Vector3.Dot(transform.forward, direction.normalized) < 0.75f) {
            print("face");
            print(elevator.name + ":" + ":" + gameObject.name + " " + Mathf.Abs(transform.position.x - player.transform.position.x));
            if (Mathf.Abs(transform.position.x - player.transform.position.x)<7.5f) {
                print("x in range");
                print(elevator.name + ":" + ":" + gameObject.name + " " + Mathf.Abs(transform.position.y - player.transform.position.y));
                if (Mathf.Abs(transform.position.y - player.transform.position.y) < 7.5f) {
                    print("y in range");
                    return true;
                }
            }
        }
        return false;
    }
    /// <summary>
    /// 控制当前拉杆控制的电梯移动和拉杆的动画
    /// </summary>
    /// <returns></returns>
    IEnumerator Move() {
        elevator.Move();
        animator.SetBool("LeverUp", true);
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("LeverUp", false);
    }
}
