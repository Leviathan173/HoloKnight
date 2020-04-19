using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SKeleton : MonoBehaviour
{
    public const float SPEED = 2.0f;
    public const float SLOW_SPEED = 1.0f;

    private Rigidbody2D _body;
    private Animator _animator;
    private BoxCollider2D _boxCollider;



    // Start is called before the first frame update
    void Start()
    {
        _body = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _boxCollider = GetComponent<BoxCollider2D>();
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
            Managers.Enemy1._isGrounded = true;
            _animator.SetBool(EAParameters.GROUNDED, true);
            Managers.Enemy1._isJumping = false;

        } else {
            Managers.Enemy1._isGrounded = false;
            Managers.Enemy1._isJumping = true;
            _animator.SetBool(EAParameters.GROUNDED, false);
        }

        // 控制碰撞
        _boxCollider.offset = new Vector2(-0.21f, -0.18f);
        _boxCollider.size = new Vector2(0.33f, 0.63f);

        // 移动
        if (Input.GetKeyDown(KeyCode.Keypad0)) {
            Managers.Enemy1.Turn(SPEED);
            Managers.Enemy1.Move(SPEED);
        }

        // 跳跃
        if (Input.GetKeyDown(KeyCode.Keypad1)) {
            Managers.Enemy1.Jump();
        }

        // 攻击A
        if (Input.GetKeyDown(KeyCode.Keypad2)) {
            Managers.Enemy1.AttackAEnter();
        }
        // 攻击B
        if (Input.GetKeyDown(KeyCode.Keypad3)) {
            Managers.Enemy1.AttackBEnter();
        }

        // 受击
        if (Input.GetKeyDown(KeyCode.Keypad4)) {
            Managers.Enemy1.GetHit(0);
        }

        // 死亡
        if (Input.GetKeyDown(KeyCode.Keypad5)) {
            Managers.Enemy1.Death();
        }
    }

    public void Enemy1_Death() {
        Destroy(gameObject, 0);
    }
}
