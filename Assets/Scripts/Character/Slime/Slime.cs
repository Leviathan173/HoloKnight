using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    public const float SPEED = 2.0f;
    public const float SLOW_SPEED = 1.0f;

    private Rigidbody2D _body;
    private Animator _animator;
    private BoxCollider2D _boxCollider;
    private ManagerRegister register;
    private static SlimeManager manager;


    // Start is called before the first frame update
    void Start() {
        register = GetComponent<ManagerRegister>();
        register.Register();
        manager = (SlimeManager)Managers.managers.GetManager(gameObject.name);

        _body = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update() {

        // 跳跃条件
        Vector3 max = _boxCollider.bounds.max;
        Vector3 min = _boxCollider.bounds.min;
        Vector2 corner1 = new Vector2(max.x, min.y - .2f);
        Vector2 corner2 = new Vector2(min.x, min.y - .3f);

        Collider2D hit = Physics2D.OverlapArea(corner1, corner2);

        if (hit != null && !hit.isTrigger) {
            manager._isGrounded = true;
            _animator.SetBool(EAParameters.GROUNDED, true);
            manager._isJumping = false;

        } else {
            manager._isGrounded = false;
            manager._isJumping = true;
            _animator.SetBool(EAParameters.GROUNDED, false);
        }

        // 控制碰撞
        _boxCollider.offset = new Vector2(-0.03f, -0.4f);
        _boxCollider.size = new Vector2(0.51f, 0.16f);

        // 移动
        if (Input.GetKeyDown(KeyCode.Keypad0)) {
            manager.Turn(SPEED);
            manager.Move(SPEED);
        }

        // 攻击A
        if (Input.GetKeyDown(KeyCode.Keypad1)) {
            manager.AttackAEnter();
        }
        // 攻击B
        if (Input.GetKeyDown(KeyCode.Keypad2) && manager._isGrounded) {
            manager.AttackBEnter();
        }
        // 攻击C
        if (Input.GetKeyDown(KeyCode.Keypad3)) {
            manager.AttackCEnter();
        }

        // 受击
        if (Input.GetKeyDown(KeyCode.Keypad4)) {
            manager.GetHit(0);
        }

        // 死亡
        if (Input.GetKeyDown(KeyCode.Keypad5)) {
            manager.Death();
        }
    }

    public void Enemy1_Death() {
        Destroy(gameObject, 0);
    }
}
