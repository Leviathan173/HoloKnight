using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    // UNDONE 打开箱子的监听与物品的掉落
    [SerializeField] GameObject item;
    private bool isOpen;
    Animator animator;


    void Start() {
        animator = GetComponent<Animator>();
        isOpen = animator.GetBool("isOpen");
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            if (CheckDistance()) {
                if (!isOpen) {
                    Open();
                }
            }
        }
    }
    /// <summary>
    /// 检测玩家是否面向物体并且在一定距离内
    /// </summary>
    /// <returns></returns>
    bool CheckDistance() {
        Vector3 direction = Managers.Player.player.transform.position - transform.position;
        //print(gameObject.name+":"+Vector3.Dot(transform.forward, direction.normalized));
        if (Vector3.Dot(transform.forward, direction.normalized) < 0.5f) {
            if (Mathf.Abs(transform.position.x - Managers.Player.player.transform.position.x) < 2.5f) {
                if (Mathf.Abs(transform.position.y - Managers.Player.player.transform.position.y) < 2.5f) {
                    return true;
                }
            }
        }
        return false;
    }
    void Open() {
        animator.SetBool("isOpen", true);
        if (item != null) {
            item = Instantiate(item);
            item.transform.position = new Vector3(transform.position.x, transform.position.y + (gameObject.GetComponent<Renderer>().bounds.size.y), transform.position.z);
            item.transform.parent = GameObject.Find("Item").transform;
            isOpen = true;
        } else {
            Debug.LogError("null item");
        }
    }
}
