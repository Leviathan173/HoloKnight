using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    [SerializeField] public bool needKey;
    bool isOpen;
    Animator animator;
    void Start() {
        animator = GetComponent<Animator>();
        isOpen = animator.GetBool("isOpen");
    }
    void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            if (!isOpen) {
                if (needKey) {
                    if (Managers.Player.hasKey) {
                        if (CheckDistance()) {
                            animator.SetBool("isOpen", true);
                            GetComponent<BoxCollider2D>().isTrigger = true;
                        }
                    }
                } else if (CheckDistance()) {
                    animator.SetBool("isOpen", true);
                    GetComponent<BoxCollider2D>().isTrigger = true;
                }
            }
        }
    }
    bool CheckDistance() {
        Vector3 direction = Managers.Player.player.transform.position - transform.position;
        print(gameObject.name + ":" + Vector3.Dot(transform.forward, direction.normalized));
        if (Vector3.Dot(transform.forward, direction.normalized) < -0.35f) {
            print("face");
            print(gameObject.name + " " + Mathf.Abs(transform.position.x - Managers.Player.player.transform.position.x));
            if (Mathf.Abs(transform.position.x - Managers.Player.player.transform.position.x) < 7.5f) {
                print("x in range");
                print(gameObject.name + " " + Mathf.Abs(transform.position.y - Managers.Player.player.transform.position.y));
                if (Mathf.Abs(transform.position.y - Managers.Player.player.transform.position.y) < 7.5f) {
                    print("y in range");
                    return true;
                }
            }
        }
        return false;
    }
}
