﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    [SerializeField] private GameObject Forward;
    [SerializeField] private EnemyDetector[] Attacks;

    public const float SPEED = 2.0f;
    public const float SLOW_SPEED = 1.0f;
    public const float MAX_HEALTH = 100.0f;
    public const float MAX_STAMINA = 100.0f;
    public const float STAMINA_INCREASEMENT = 0.25f;

    private Rigidbody2D _body;
    private Animator _animator;
    private BoxCollider2D _boxCollider;
    private Vector2 collSize;
    private Vector2 collOffset;
    private ManagerRegister register;
    private static EnemyManager manager;

    private float _width;

    // Start is called before the first frame update
    void Start() {
        if(manager != null) {
            
        } else {
            register = GetComponent<ManagerRegister>();
            register.Register();
            manager = (EnemyManager)Managers.managers.GetManager(gameObject.name);

            _body = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _boxCollider = GetComponent<BoxCollider2D>();

            _width = GetComponent<SpriteRenderer>().bounds.size.x / 3;

            collSize = _boxCollider.size;
            collOffset = _boxCollider.offset;

            manager.InitComponents(gameObject, _body, _animator, Forward, Attacks, _width);
            manager.InitStats(MAX_HEALTH, MAX_STAMINA, STAMINA_INCREASEMENT);

        }
        
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
        _boxCollider.offset = collOffset;
        _boxCollider.size = collSize;

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
