using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour
{
    public const float SPEED = 2.0f;
    public const float SLOW_SPEED = 1.0f;

    private Rigidbody2D _body;
    private Animator _animator;
    private BoxCollider2D _boxCollider;
    private Vector2 collSize;
    private Vector2 collOffset;
    private ManagerRegister register;
    private static SkeletonManager skeletonManager;


    // Start is called before the first frame update
    void Start()
    {
        register = GetComponent<ManagerRegister>();
        register.Register();
        skeletonManager = (SkeletonManager)Managers.managers.GetManager(gameObject.name);

        _body = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _boxCollider = GetComponent<BoxCollider2D>();

        collSize = _boxCollider.size;
        collOffset = _boxCollider.offset;
    }

    // Update is called once per frame
    void Update()
    {
        // 跳跃条件
        Vector3 max = _boxCollider.bounds.max;
        Vector3 min = _boxCollider.bounds.min;
        Vector2 corner1 = new Vector2(max.x, min.y - .2f);
        Vector2 corner2 = new Vector2(min.x, min.y - .3f);

        Collider2D hit = Physics2D.OverlapArea(corner1, corner2);

        if (hit != null && !hit.isTrigger) {
            skeletonManager._isGrounded = true;
            _animator.SetBool(EAParameters.GROUNDED, true);
            skeletonManager._isJumping = false;

        } else {
            skeletonManager._isGrounded = false;
            skeletonManager._isJumping = true;
            _animator.SetBool(EAParameters.GROUNDED, false);
        }

        // 控制碰撞
        _boxCollider.offset = collOffset;
        _boxCollider.size = collSize;

        // 移动
        if (Input.GetKeyDown(KeyCode.M)) {
            print("skeleton manager:" + skeletonManager + "skeleton name" + gameObject.name);
            skeletonManager.Turn(SPEED);
            skeletonManager.Move(SPEED);
        }

        // 跳跃
        if (Input.GetKeyDown(KeyCode.Keypad1)) {
            skeletonManager.Jump();
        }

        // 攻击A
        if (Input.GetKeyDown(KeyCode.Keypad2)) {
            skeletonManager.AttackAEnter();
        }
        // 攻击B
        if (Input.GetKeyDown(KeyCode.Keypad3)) {
            skeletonManager.AttackBEnter();
        }

        // 受击
        if (Input.GetKeyDown(KeyCode.Keypad4)) {
            skeletonManager.GetHit(0);
        }

        // 死亡
        if (Input.GetKeyDown(KeyCode.Keypad5)) {
            skeletonManager.Death();
        }
    }

    public void Enemy1_Death() {
        Destroy(gameObject, 0);
    }
}
